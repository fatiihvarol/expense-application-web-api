using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Web.Api.Data.ViewModels.Authentication;
using Microsoft.Extensions.Configuration;
using System;
using Web.Api.Data.Entities;

namespace Web.Api.Services
{
    public class TokenHandler
    {
        private readonly IConfiguration _configuration;

        public TokenHandler(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public AuthResponseVM GenerateToken(User user )
        {
            var tokenExpirationInMinutes = int.Parse(_configuration["Token:TokenExpirationInMinutes"]);

            Claim[] claims = GetClaims(user);


            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Token:Secret"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _configuration["Token:Issuer"],
                audience: _configuration["Token:Audience"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(tokenExpirationInMinutes),
                signingCredentials: creds
            );

            var refreshToken = GenerateRefreshToken();

            return new AuthResponseVM
            {
                Token = new JwtSecurityTokenHandler().WriteToken(token),
                RefreshToken = refreshToken,
                ExpiresAt = token.ValidTo
            };
        }

        private Claim[] GetClaims(ApplicationUser user)
        {
            var claims = new[]
            {
            new Claim("Id", user.Id.ToString()),
            new Claim("Email", user.Email),
            new Claim(ClaimTypes.Role, user.Role.ToString())
        };

            return claims;
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
    }
}
