using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Web.Api.Base.Response;
using Web.Api.Schema;
using Web.Api.Data.Entities;
using Web.Api.Data.AppDbContext;
using Web.Api.Business.Cqrs;
using Web.Api.Base.Enums;
using Web.Api.Base.Message; // Replace with your actual namespace

namespace Web.Api.Business.Query.ExpenseFormQuery
{
    public class ExpenseFormQueryHandler
        : IRequestHandler<GetExpenseByIdQuery, ApiResponse<ExpenseFormResponse>>,
          IRequestHandler<GetMyExpensesQuery, ApiResponse<List<ExpenseFormResponse>>>,
        IRequestHandler<GetExpenseFormsByManager, ApiResponse<List<ExpenseFormResponse>>>,
        IRequestHandler<GetExpenseFormsByAccountant, ApiResponse<List<ExpenseFormResponse>>>,
        IRequestHandler<GetEmployeeExpenseInfoQuery, ApiResponse<EmployeeExpenseInfoVM>>,
        IRequestHandler<GetExpenseFormsByAdmin, ApiResponse<List<ExpenseFormResponse>>>

    {
        private readonly AppDbContext _dbContext;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ExpenseFormQueryHandler(AppDbContext dbContext, IMapper mapper, IHttpContextAccessor httpContextAccessor)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
        }
        private string? GetUserId()
        {
            return _httpContextAccessor.HttpContext?.User.Claims.FirstOrDefault(c => c.Type == "Id")?.Value;
        }


        public async Task<ApiResponse<ExpenseFormResponse>> Handle(GetExpenseByIdQuery request, CancellationToken cancellationToken)
        {
            var userId = GetUserId();
            if (userId == null)
            {
                return ApiResponse<ExpenseFormResponse>.Failure(ErrorMessage.TokenErrorMessage.UserIdNotFound);
            }

            var user = _dbContext.VpApplicationUsers.FirstOrDefault(u => u.Id == Int32.Parse(userId));
            if (user == null)
            {
                return
                    ApiResponse<ExpenseFormResponse>.Failure(ErrorMessage.TokenErrorMessage.UserNotFound);
            }
            // Adjusted query to include deleted forms for Admin role
            var expenseForm = await _dbContext.VpExpenseForms
                .Include(e => e.Expenses!)
                .ThenInclude(expense => expense.Category)
                .FirstOrDefaultAsync(e => e.Id == request.Id && (user.Role == UserRoleEnum.Admin || e.IsDeleted == false), cancellationToken);

            if (expenseForm == null)
            {
                return ApiResponse<ExpenseFormResponse>.Failure(ErrorMessage.ExpenseFormErrorMessage.ExpenseFormNotFound);
            }


            var response = _mapper.Map<ExpenseFormResponse>(expenseForm);
            return ApiResponse<ExpenseFormResponse>.Success(response);
        }


        public async Task<ApiResponse<List<ExpenseFormResponse>>> Handle(GetMyExpensesQuery request, CancellationToken cancellationToken)
        {
            var userId = GetUserId();

            if (userId == null)
            {
                return ApiResponse<List<ExpenseFormResponse>>.Failure(ErrorMessage.TokenErrorMessage.UserIdNotFound);
            }
            var expenseForms = await _dbContext.VpExpenseForms
         .Where(e => e.IsDeleted == false)
         .Where(e => e.EmployeeId == int.Parse(userId))
         .Include(e => e.Expenses!)                    // Include Expenses
         .ThenInclude(expense => expense.Category)    // ThenInclude Category in each Expense
         .ToListAsync(cancellationToken);



            var response = _mapper.Map<List<ExpenseFormResponse>>(expenseForms);
            return ApiResponse<List<ExpenseFormResponse>>.Success(response);
        }

        public Task<ApiResponse<List<ExpenseFormResponse>>> Handle(GetExpenseFormsByManager request, CancellationToken cancellationToken)
        {
            var userId = GetUserId();

            if (userId is null)
                return Task.FromResult(ApiResponse<List<ExpenseFormResponse>>.Failure(ErrorMessage.TokenErrorMessage.UserIdNotFound));

            var expenseForms = _dbContext.VpExpenseForms
                .Where(e => e.IsDeleted == false)
                .Where(e => e.ManagerId == Int32.Parse(userId))
                .Where(e => e.ExpenseStatusEnum == ExpenseStatusEnum.Pending)
                .Include(e => e.Expenses!)
                .ThenInclude(expense => expense.Category)
                .ToList();

            var response = _mapper.Map<List<ExpenseFormResponse>>(expenseForms);

            foreach (var item in response)
            {
                item.EmployeeFullName = _dbContext.VpEmployees.FirstOrDefault(e => e.Id == item.EmployeeId)?.Name + " " + _dbContext.VpEmployees.FirstOrDefault(e => e.Id == item.EmployeeId)?.Surname;
            }

            return Task.FromResult(ApiResponse<List<ExpenseFormResponse>>.Success(response));

        }

        public Task<ApiResponse<List<ExpenseFormResponse>>> Handle(GetExpenseFormsByAccountant request, CancellationToken cancellationToken)
        {
            var userId = GetUserId();

            if (userId is null)
                return Task.FromResult(ApiResponse<List<ExpenseFormResponse>>.Failure(ErrorMessage.TokenErrorMessage.UserIdNotFound));

            var expenseForms = _dbContext.VpExpenseForms
                .Where(e => e.IsDeleted == false)
                .Where(e => e.ExpenseStatusEnum == ExpenseStatusEnum.Approved)
                .Include(e => e.Expenses!)
                .ThenInclude(expense => expense.Category)
                .ToList();

            var response = _mapper.Map<List<ExpenseFormResponse>>(expenseForms);

            foreach (var item in response)
            {
                item.EmployeeFullName = _dbContext.VpEmployees.FirstOrDefault(e => e.Id == item.EmployeeId)?.Name + " " + _dbContext.VpEmployees.FirstOrDefault(e => e.Id == item.EmployeeId)?.Surname;
            }

            foreach (var item in response)
            {
                item.ManagerFullName = _dbContext.VpManagers.FirstOrDefault(e => e.Id == item.ManagerId)?.Name + " " + _dbContext.VpManagers.FirstOrDefault(e => e.Id == item.ManagerId)?.Surname;

            }
            return Task.FromResult(ApiResponse<List<ExpenseFormResponse>>.Success(response));
        }

        public Task<ApiResponse<EmployeeExpenseInfoVM>> Handle(GetEmployeeExpenseInfoQuery request, CancellationToken cancellationToken)
        {
            var userId = GetUserId();

            if (userId is null)
                return Task.FromResult(ApiResponse<EmployeeExpenseInfoVM>.Failure(ErrorMessage.TokenErrorMessage.UserIdNotFound));

            var expenseForms = _dbContext.VpExpenseForms
                .Where(e => !e.IsDeleted && e.EmployeeId == Int32.Parse(userId))
                .Include(e => e.Expenses!)
                .ThenInclude(expense => expense.Category)
                .OrderByDescending(e => e.CreatedDate)
                .ToList();

            // Her para birimi için toplam gider miktarını hesaplama
            var totalExpenseAmountByCurrency = expenseForms
                .GroupBy(e => e.CurrencyEnum) // Para birimine göre gruplama
                .ToDictionary(
                    g => g.Key,
                    g => g.Sum(e => e.TotalAmount) // Para birimine göre toplam miktar
                );

            // Son 3 expense formu alıyoruz
            var temp = expenseForms
                .Take(3)
                .ToList();
            var lastThreeExpenseForms = _mapper.Map<ICollection<ExpenseFormResponse>>(temp);


            var response = new EmployeeExpenseInfoVM
            {
                PendingApprovalCount = expenseForms.Count(e => e.ExpenseStatusEnum == ExpenseStatusEnum.Pending),
                ApprovedCount = expenseForms.Count(e => e.ExpenseStatusEnum == ExpenseStatusEnum.Approved),
                TotalExpenseCount = expenseForms.Count,
                TotalExpenseAmount = totalExpenseAmountByCurrency, // Para birimine göre toplamları ekleme
                LastExpenseForms = lastThreeExpenseForms // Son 3 formu ekleme
            };

            return Task.FromResult(ApiResponse<EmployeeExpenseInfoVM>.Success(response));
        }

        public Task<ApiResponse<List<ExpenseFormResponse>>> Handle(GetExpenseFormsByAdmin request, CancellationToken cancellationToken)
        {
            var userId = GetUserId();

            if (userId is null)
                return Task.FromResult(ApiResponse<List<ExpenseFormResponse>>.Failure(ErrorMessage.TokenErrorMessage.UserIdNotFound));

            var expenseForms = _dbContext.VpExpenseForms
                .Include(e => e.Expenses!)
                .ThenInclude(expense => expense.Category)
                .ToList();

            var response = _mapper.Map<List<ExpenseFormResponse>>(expenseForms);

            foreach (var item in response)
            {
                item.EmployeeFullName = _dbContext.VpEmployees.FirstOrDefault(e => e.Id == item.EmployeeId)?.Name + " " + _dbContext.VpEmployees.FirstOrDefault(e => e.Id == item.EmployeeId)?.Surname;
            }

            foreach (var item in response)
            {
                item.ManagerFullName = _dbContext.VpManagers.FirstOrDefault(e => e.Id == item.ManagerId)?.Name + " " + _dbContext.VpManagers.FirstOrDefault(e => e.Id == item.ManagerId)?.Surname;

            }
            return Task.FromResult(ApiResponse<List<ExpenseFormResponse>>.Success(response));
        }
    }
}
