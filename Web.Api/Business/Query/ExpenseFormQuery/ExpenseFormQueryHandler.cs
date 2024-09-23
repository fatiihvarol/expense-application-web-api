using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Web.Api.Base.Response;
using Web.Api.Schema;
using Web.Api.Data.Entities;
using Web.Api.Data.AppDbContext;
using Web.Api.Business.Cqrs;
using Web.Api.Base.Enums; // Replace with your actual namespace

namespace Web.Api.Business.Query.ExpenseFormQuery
{
    public class ExpenseFormQueryHandler
        : IRequestHandler<GetAllExpensesQuery, ApiResponse<List<ExpenseFormResponse>>>,
          IRequestHandler<GetExpensesByEmployeeIdQuery, ApiResponse<List<ExpenseFormResponse>>>,
          IRequestHandler<GetExpenseByIdQuery, ApiResponse<ExpenseFormResponse>>,
          IRequestHandler<GetExpensesByParametersQuery, ApiResponse<List<ExpenseFormResponse>>>,
          IRequestHandler<GetMyExpensesQuery, ApiResponse<List<ExpenseFormResponse>>>,
        IRequestHandler<GetExpenseFormsByManager, ApiResponse<List<ExpenseFormResponse>>>,
        IRequestHandler<GetExpenseFormsByAccountant, ApiResponse<List<ExpenseFormResponse>>>,
        IRequestHandler<GetEmployeeExpenseInfoQuery, ApiResponse<EmployeeExpenseInfoResponse>>
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

        public async Task<ApiResponse<List<ExpenseFormResponse>>> Handle(GetAllExpensesQuery request, CancellationToken cancellationToken)
        {
            var expenseForms = await _dbContext.VpExpenseForms
                .Where(e => e.IsDeleted == false)
                .Include(e => e.Expenses)
                .ToListAsync(cancellationToken);

            var response = _mapper.Map<List<ExpenseFormResponse>>(expenseForms);
            return ApiResponse<List<ExpenseFormResponse>>.Success(response);
        }

        public async Task<ApiResponse<List<ExpenseFormResponse>>> Handle(GetExpensesByEmployeeIdQuery request, CancellationToken cancellationToken)
        {
            var expenseForms = await _dbContext.VpExpenseForms
                .Where(e => e.IsDeleted == false)
                .Where(e => e.EmployeeId == request.EmployeeId)
                .Include(e => e.Expenses) 
                .ToListAsync(cancellationToken);

            var response = _mapper.Map<List<ExpenseFormResponse>>(expenseForms);
            return ApiResponse<List<ExpenseFormResponse>>.Success(response);
        }

        public async Task<ApiResponse<ExpenseFormResponse>> Handle(GetExpenseByIdQuery request, CancellationToken cancellationToken)
        {
            var userId = _httpContextAccessor.HttpContext?.User.Claims.FirstOrDefault(c => c.Type == "Id")?.Value;
            if (userId == null)
            {
                return   ApiResponse<ExpenseFormResponse>.Failure("User  Id Not Found in Token");
            }
            var user = _dbContext.VpApplicationUsers.FirstOrDefault(u => u.Id == Int32.Parse(userId));
            
            var expenseForm = await _dbContext.VpExpenseForms
                .Include(e => e.Expenses) 
                .Where(e=>e.IsDeleted==false)
                .FirstOrDefaultAsync(e => e.Id == request.Id, cancellationToken);

        
            if (expenseForm == null)
            {
                return ApiResponse<ExpenseFormResponse>.Failure("Expense form not found");
            }
            if (user != null && (user.Role == UserRoleEnum.Manager || user.Role == UserRoleEnum.Employee))
            {
                if (expenseForm.ManagerId != Int32.Parse(userId) && expenseForm.EmployeeId != Int32.Parse(userId))
                {
                    return ApiResponse<ExpenseFormResponse>.Failure("You don't have permission");
                }
                if (user.Role == UserRoleEnum.Manager && expenseForm.ExpenseStatusEnum != ExpenseStatusEnum.Pending)
                {
                    return ApiResponse<ExpenseFormResponse>.Failure("You don't have permission");

                }
            }
            var response = _mapper.Map<ExpenseFormResponse>(expenseForm);
            return ApiResponse<ExpenseFormResponse>.Success(response);
        }

        public async Task<ApiResponse<List<ExpenseFormResponse>>> Handle(GetExpensesByParametersQuery request, CancellationToken cancellationToken)
        {

            Enum.TryParse(request.Status, out ExpenseStatusEnum statusEnum);
           

            var expenseForms = await _dbContext.VpExpenseForms
                .Where(e => e.IsDeleted == false)
                .Where(e =>
                    (request.EmployeeId == 0 || e.EmployeeId == request.EmployeeId) &&
                    (request.Status == null || e.ExpenseStatusEnum == statusEnum) &&
                    (request.Amount == 0 || e.TotalAmount == request.Amount))
                .Include(e => e.Expenses) 
                .ToListAsync(cancellationToken);



            var response = _mapper.Map<List<ExpenseFormResponse>>(expenseForms);
            return ApiResponse<List<ExpenseFormResponse>>.Success(response);
        }

        public async Task<ApiResponse<List<ExpenseFormResponse>>> Handle(GetMyExpensesQuery request, CancellationToken cancellationToken)
        {
            var userId = _httpContextAccessor.HttpContext?.User.Claims.FirstOrDefault(c => c.Type == "Id")?.Value;

            if (userId == null)
            {
                return ApiResponse<List<ExpenseFormResponse>>.Failure("User Id not found in token");
            }
            var expenseForms = await _dbContext.VpExpenseForms
                .Where(e => e.IsDeleted == false)
                .Where(e => e.EmployeeId == Int32.Parse(userId))
                .Include(e => e.Expenses) 
                .ToListAsync(cancellationToken);

            var response = _mapper.Map<List<ExpenseFormResponse>>(expenseForms);
            return ApiResponse<List<ExpenseFormResponse>>.Success(response);
        }

        public Task<ApiResponse<List<ExpenseFormResponse>>> Handle(GetExpenseFormsByManager request, CancellationToken cancellationToken)
        {
            var userId = _httpContextAccessor.HttpContext?.User.Claims.FirstOrDefault(c => c.Type == "Id")?.Value;

            if (userId is null)       
                return Task.FromResult(ApiResponse<List<ExpenseFormResponse>>.Failure("User Id not found in token"));
            
            var expenseForms = _dbContext.VpExpenseForms
                .Where(e => e.IsDeleted == false)
                .Where(e => e.ManagerId == Int32.Parse(userId))
                .Where(e => e.ExpenseStatusEnum == ExpenseStatusEnum.Pending)
                .Include(e => e.Expenses)
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
            var userId = _httpContextAccessor.HttpContext?.User.Claims.FirstOrDefault(c => c.Type == "Id")?.Value;

            if (userId is null)
                return Task.FromResult(ApiResponse<List<ExpenseFormResponse>>.Failure("User Id not found in token"));

            var expenseForms = _dbContext.VpExpenseForms
                .Where(e => e.IsDeleted == false)
                .Where(e => e.ExpenseStatusEnum == ExpenseStatusEnum.Approved)
                .Include(e => e.Expenses)
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

        public  Task<ApiResponse<EmployeeExpenseInfoResponse>> Handle(GetEmployeeExpenseInfoQuery request, CancellationToken cancellationToken)
        {
            var userId = _httpContextAccessor.HttpContext?.User.Claims.FirstOrDefault(c => c.Type == "Id")?.Value;

            if (userId is null)
                return Task.FromResult(ApiResponse<EmployeeExpenseInfoResponse>.Failure("User Id not found in token"));

            var expenseForms = _dbContext.VpExpenseForms
                .Where(e => !e.IsDeleted && e.EmployeeId == Int32.Parse(userId))
                .Include(e => e.Expenses)
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


            var response = new EmployeeExpenseInfoResponse
            {
                PendingApprovalCount = expenseForms.Count(e => e.ExpenseStatusEnum == ExpenseStatusEnum.Pending),
                ApprovedCount = expenseForms.Count(e => e.ExpenseStatusEnum == ExpenseStatusEnum.Approved),
                TotalExpenseCount = expenseForms.Count,
                TotalExpenseAmount = totalExpenseAmountByCurrency, // Para birimine göre toplamları ekleme
                LastExpenseForms = lastThreeExpenseForms // Son 3 formu ekleme
            };

            return Task.FromResult(ApiResponse<EmployeeExpenseInfoResponse>.Success(response));
        }

    }
}
