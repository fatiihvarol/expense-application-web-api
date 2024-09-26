using Web.Api.Base.Response;
using MediatR;
using Web.Api.Schema.Authentication;

namespace Web.Api.Business.Cqrs
{
    public record CreateTokenCommand(LoginRequest Model) : IRequest<ApiResponse<AuthResponseVM>>;
    public record RefreshTokenCommand(RefreshTokenRequest Model) : IRequest<ApiResponse<AuthResponseVM>>;

}
