using MediatR;
using Web.Api.Base.Response;
using Web.Api.Schema;
using Web.Api.Schema.Authentication;

namespace Web.Api.Business.Cqrs
{
    public record ExpenseCategoryCreateCommand(ExpenseCategoryCreateRequest Model) : IRequest<ApiResponse<ExpenseCategoryResponse>>;

    public record GetAllExpenseCategoryQuery() : IRequest<ApiResponse<List<ExpenseCategoryResponse>>>;

}
