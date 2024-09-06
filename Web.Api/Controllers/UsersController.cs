using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Web.Api.Data.AppDbContext;
using Web.Api.Data.Entities;

namespace Web.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly AppDbContext _appDbContext;
        public UsersController(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }
        [HttpPost]

        public IActionResult AddUser(User user)
        {
            _appDbContext.Users.Add(user);
            _appDbContext.SaveChanges();
            return Ok(user);

        }

        [HttpGet]

        public IActionResult GetUser(AppUserVM user2)
        {
           var user = _appDbContext.Users.Where(x=>x.Email == user2.Email).FirstOrDefault();
            return Ok();


        }
        public class AppUserVM

        {
            public string Email { get; set; }
            public string Password { get; set; }
        }
    }
}
