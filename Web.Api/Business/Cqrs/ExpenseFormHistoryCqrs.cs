using MediatR;
using Web.Api.Base.Response;
using Web.Api.Schema;

namespace Web.Api.Business.Cqrs
{
    public record GetExpenseFormHistoryQuery(int ExpenseFormId) : IRequest<ApiResponse<List<ExpenseFormHistoryVM>>>;

}
