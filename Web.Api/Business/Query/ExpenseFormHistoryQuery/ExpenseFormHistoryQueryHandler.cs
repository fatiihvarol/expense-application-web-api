using AutoMapper;
using MediatR;
using Web.Api.Base.Response;
using Web.Api.Business.Cqrs;
using Web.Api.Data.AppDbContext;
using Web.Api.Schema;

namespace Web.Api.Business.Query.ExpenseFormHistoryQuery
{
    public class ExpenseFormHistoryQueryHandler :
        IRequestHandler<GetExpenseFormHistoryQuery, ApiResponse<List<ExpenseFormHistoryVM>>>
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public ExpenseFormHistoryQueryHandler(AppDbContext dbContext, IMapper mapper)
        {

            _context = dbContext;
            _mapper = mapper;
        }

        public Task<ApiResponse<List<ExpenseFormHistoryVM>>> Handle(GetExpenseFormHistoryQuery request, CancellationToken cancellationToken)
        {
            var expenseFormHistory = _context.VpExpenseFormHistories.Where(x => x.ExpenseFormId == request.ExpenseFormId).ToList();
            var response = _mapper.Map<List<ExpenseFormHistoryVM>>(expenseFormHistory);

            foreach (var item in response)
            {
                item.FullName = _context.VpApplicationUsers.Where(x => x.Id == item.MadeBy).FirstOrDefault()?.Name + " " + _context.VpApplicationUsers.Where(x => x.Id == item.MadeBy).FirstOrDefault()?.Surname;
                item.Date = item.Date.ToLocalTime();
            }

            return Task.FromResult(ApiResponse<List<ExpenseFormHistoryVM>>.Success(response));

        }
    }
}
