using Web.Api.Base.Response;
using MediatR;
using Web.Api.Schema.Authentication;

namespace Web.Api.Business.Cqrs
{
    public record CreateTokenCommand(LoginVM Model) : IRequest<ApiResponse<AuthResponseVM>>;
}
