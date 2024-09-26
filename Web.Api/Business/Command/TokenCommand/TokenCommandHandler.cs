using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Web.Api.Base.Encryption;
using Web.Api.Base.Response;
using Web.Api.Business.Cqrs;
using Web.Api.Data.AppDbContext;
using Web.Api.Data.Entities;
using Web.Api.Schema.Authentication;

namespace Web.Api.Business.Command.TokenCommand
{
    public class TokenCommandHandler :
        IRequestHandler<CreateTokenCommand, ApiResponse<AuthResponseVM>>,
        IRequestHandler<RefreshTokenCommand, ApiResponse<AuthResponseVM>>
    {
        private readonly IConfiguration _configuration;
        private readonly AppDbContext _dbContext;

        public TokenCommandHandler(IConfiguration configuration, AppDbContext dbContext)
        {
            _configuration = configuration;
            _dbContext = dbContext;
        }

        public async Task<ApiResponse<AuthResponseVM>> Handle(CreateTokenCommand request, CancellationToken cancellationToken)
        {
            // Validate the request model
            if (string.IsNullOrEmpty(request.Model.Password) || string.IsNullOrEmpty(request.Model.Email))
            {
                return ApiResponse<AuthResponseVM>.Failure("Model can not be empty");
            }

            var hashedPassword = Md5Extension.Create(request.Model.Password);

            // Fetch user from the database
            var user = await _dbContext.VpApplicationUsers
                .FirstOrDefaultAsync(u => u.Email == request.Model.Email && u.Password == hashedPassword);

            // Handle case where user is not found
            if (user == null)
            {
                return ApiResponse<AuthResponseVM>.Failure("User not found");
            }

            // Token creation logic
            var tokenExpirationInMinutes = int.Parse(_configuration["Token:TokenExpirationInMinutes"]);
            Claim[] claims = GetClaims(user);

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Token:Secret"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _configuration["Token:Issuer"],
                audience: _configuration["Token:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(tokenExpirationInMinutes),
                signingCredentials: creds
            );

            var refreshToken = GenerateRefreshToken();

            // Save the refresh token to the database
            var vpRefreshToken = new VpRefreshToken
            {
                Token = refreshToken,
                ExpiresAt = DateTime.UtcNow.AddDays(30), // Örneğin 30 gün geçerlilik süresi
                UserId = user.Id
            };

            await _dbContext.VpRefreshTokens.AddAsync(vpRefreshToken);
            await _dbContext.SaveChangesAsync(cancellationToken); // Değişiklikleri kaydet

            // Prepare the response model
            var response = new AuthResponseVM
            {
                Token = new JwtSecurityTokenHandler().WriteToken(token),
                RefreshToken = refreshToken,
                ExpiresAt = token.ValidTo
            };

            // Return a successful response
            return ApiResponse<AuthResponseVM>.Success(response);
        }

        private Claim[] GetClaims(VpApplicationUser user)
        {
            return new[]
            {
                new Claim("Id", user.Id.ToString()),
                new Claim("Email", user.Email),
                new Claim(ClaimTypes.Role, user.Role.ToString())
            };
        }

        private string GenerateRefreshToken()
        {
            var randomNumber = new byte[32];
            using (var rng = System.Security.Cryptography.RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomNumber);
                return Convert.ToBase64String(randomNumber);
            }
        }

        public async Task<ApiResponse<AuthResponseVM>> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
        {
            // Refresh token kontrolü
            var refreshToken = await _dbContext.VpRefreshTokens
                .FirstOrDefaultAsync(rt => rt.Token == request.Model.RefreshToken && !rt.IsRevoked && rt.ExpiresAt > DateTime.UtcNow);

            if (refreshToken == null)
            {
                return ApiResponse<AuthResponseVM>.Failure("Invalid refresh token.");
            }

            // Kullanıcı bilgilerini al
            var user = await _dbContext.VpApplicationUsers.FindAsync(refreshToken.UserId);
            if (user == null)
            {
                return ApiResponse<AuthResponseVM>.Failure("User not found.");
            }

            // Yeni token oluştur
            var tokenExpirationInMinutes = int.Parse(_configuration["Token:TokenExpirationInMinutes"]);
            Claim[] claims = GetClaims(user);

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Token:Secret"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var newToken = new JwtSecurityToken(
                issuer: _configuration["Token:Issuer"],
                audience: _configuration["Token:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(tokenExpirationInMinutes),
                signingCredentials: creds
            );

            var newRefreshToken = GenerateRefreshToken();

            // Eski refresh token'ı geçersiz kıl
            refreshToken.IsRevoked = true;
            _dbContext.VpRefreshTokens.Update(refreshToken);
            await _dbContext.SaveChangesAsync(cancellationToken); 
            var refreshTokenExpirationInDays = int.Parse(_configuration["Token:RefreshTokenExpirationInDays"]);
            // Yeni refresh token'ı kaydet
            var vpRefreshToken = new VpRefreshToken
            {
                Token = newRefreshToken,
                ExpiresAt = DateTime.UtcNow.AddDays(refreshTokenExpirationInDays), 
                UserId = user.Id
            };

            await _dbContext.VpRefreshTokens.AddAsync(vpRefreshToken);
            await _dbContext.SaveChangesAsync(cancellationToken); 

            // Yanıt modeli hazırla
            var response = new AuthResponseVM
            {
                Token = new JwtSecurityTokenHandler().WriteToken(newToken),
                RefreshToken = newRefreshToken,
                ExpiresAt = newToken.ValidTo
            };

            return ApiResponse<AuthResponseVM>.Success(response);
        }
    }
}
