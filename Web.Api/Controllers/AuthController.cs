using Microsoft.AspNetCore.Mvc;
using Web.Api.Services;
using Web.Api.Data.AppDbContext;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using Web.Api.Schema.Authentication;
using MediatR;
using Web.Api.Base.Response;
using Web.Api.Business.Cqrs;
using Web.Api.Schema;

namespace Web.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AuthController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("login")]
        public async Task<ApiResponse<AuthResponseVM>> Post([FromBody] LoginVM request)
        {
            var operation = new CreateTokenCommand(request);
            var result = await _mediator.Send(operation);
            return result;
        }

    }
}
