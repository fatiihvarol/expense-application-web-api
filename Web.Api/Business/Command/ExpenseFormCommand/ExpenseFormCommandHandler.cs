using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;
using Web.Api.Base.Response;
using Web.Api.Data.Entities;
using Web.Api.Schema;
using Web.Api.Data.AppDbContext;
using Web.Api.Business.Cqrs;
using Web.Api.Base.Enums;

namespace Web.Api.Business.Command.ExpenseFormCommand
{
    public class ExpenseFormCommandHandler
        : IRequestHandler<CreateExpenseFormCommand, ApiResponse<ExpenseFormResponse>>,
          IRequestHandler<UpdateExpenseFormCommand, ApiResponse<ExpenseFormResponse>>,
          IRequestHandler<DeleteExpenseFormCommand, ApiResponse<object>>,
          IRequestHandler<DeclineExpenseFormCommand, ApiResponse<object>>
    {
        private readonly AppDbContext _dbContext;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ExpenseFormCommandHandler(AppDbContext dbContext, IMapper mapper, IHttpContextAccessor httpContextAccessor)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<ApiResponse<ExpenseFormResponse>> Handle(CreateExpenseFormCommand request, CancellationToken cancellationToken)
        {
            // JWT token'dan User Id'yi çekme
            var userId = _httpContextAccessor.HttpContext?.User.Claims.FirstOrDefault(c => c.Type == "Id")?.Value;

            if (userId == null)
            {
                return ApiResponse<ExpenseFormResponse>.Failure("User Id not found in token");
            }

            // User Id ile veritabanında Employee ve Manager bilgilerini sorgulama
            var employee = await _dbContext.VpEmployees.FirstOrDefaultAsync(e => e.Id == Int32.Parse(userId), cancellationToken);
            if (employee == null)
            {
                return ApiResponse<ExpenseFormResponse>.Failure("Employee not found");
            }

            var manager = await _dbContext.VpManagers.FirstOrDefaultAsync(m => m.Id == employee.ManagerId, cancellationToken);
            if (manager == null)
            {
                return ApiResponse<ExpenseFormResponse>.Failure("Manager not found");
            }

            if (request.Model.Expenses == null || request.Model.Expenses.Count == 0)
            {
                return ApiResponse<ExpenseFormResponse>.Failure("You can not add empty expense");
            }

            var expenseForm = _mapper.Map<VpExpenseForm>(request);
            expenseForm.EmployeeId = employee.Id;  
            expenseForm.ManagerId = manager.Id;  
            expenseForm.ExpenseStatusEnum = ExpenseStatusEnum.Pending;
            expenseForm.CreatedDate = DateTime.Now;
            expenseForm.CreateBy = userId;

            var fromdb = _dbContext.VpExpenseForms.Add(expenseForm);
            await _dbContext.SaveChangesAsync(cancellationToken);

            var response = _mapper.Map<ExpenseFormResponse>(expenseForm);
            return ApiResponse<ExpenseFormResponse>.Success(response);
        }


        public async Task<ApiResponse<ExpenseFormResponse>> Handle(UpdateExpenseFormCommand request, CancellationToken cancellationToken)
        {
            var expenseForm = await _dbContext.VpExpenseForms.FindAsync(request.Id);
            if (expenseForm == null)
            {
                return ApiResponse<ExpenseFormResponse>.Failure("Expense form not found");
            }

            _mapper.Map(request, expenseForm);
            _dbContext.VpExpenseForms.Update(expenseForm);
            await _dbContext.SaveChangesAsync(cancellationToken);

            var response = _mapper.Map<ExpenseFormResponse>(expenseForm);
            return ApiResponse<ExpenseFormResponse>.Success(response);
        }

        public async Task<ApiResponse<object>> Handle(DeleteExpenseFormCommand request, CancellationToken cancellationToken)
        {
            var expenseForm = await _dbContext.VpExpenseForms.FindAsync(request.Id, cancellationToken);
            if (expenseForm == null)
            {
                return ApiResponse<object>.Failure("Expense form not found");
            }

            expenseForm.IsDeleted = true;
            await _dbContext.SaveChangesAsync(cancellationToken);

            return ApiResponse<object>.Success("Expense Form Deleted");
        }

        public async Task<ApiResponse<object>> Handle(DeclineExpenseFormCommand request, CancellationToken cancellationToken)
        {
            var expenseForm = await _dbContext.VpExpenseForms.FirstOrDefaultAsync(e => e.Id == request.Id, cancellationToken);
            if (expenseForm == null)
            {
                return ApiResponse<object>.Failure("Expense form not found");
            }

            expenseForm.ExpenseStatusEnum = Base.Enums.ExpenseStatusEnum.Rejected;
            expenseForm.RejectionDescription = request.RejectionDescription;
            _dbContext.VpExpenseForms.Update(expenseForm);
            await _dbContext.SaveChangesAsync(cancellationToken);

            return ApiResponse<object>.Success("Expense Form Declined");
        }
    }
}
