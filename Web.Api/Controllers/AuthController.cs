using Microsoft.AspNetCore.Mvc;
using Web.Api.Services;
using Web.Api.Data.ViewModels.Authentication;
using Web.Api.Data.AppDbContext;

namespace Web.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly TokenHandler _tokenHandler;
        private readonly AppDbContext _dbContext;

        public AuthController(TokenHandler tokenHandler, AppDbContext dbContext)
        {
            _tokenHandler = tokenHandler;
            _dbContext = dbContext;
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginVM loginVM)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = _dbContext.Users.FirstOrDefault(u => u.Email == loginVM.Email && u.Password == loginVM.Password);
            if (user != null)
            {   
                var tokenResponse = _tokenHandler.GenerateToken(user);
                return Ok(tokenResponse);
            }
            return BadRequest(ModelState);

            
        }
    }
}
