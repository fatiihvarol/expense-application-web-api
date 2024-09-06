using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Web.Api.Data.AppDbContext;
using Web.Api.Data.Entities;

namespace Web.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
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

        public IActionResult GetUser()
        {
            return Ok("deneme");


        }
       
    }
}
