using Web.Api.Base.Enums;

namespace Web.Api.Schema
{
    public class ExpenseFormRequest
    {
    }
    public class ExpenseFormResponse
    {
        public int Id { get; set; }
        public string? Description { get; set; }

        public decimal TotalAmount { get; set; }

        public string? CurrencyEnum { get; set; }

        public ExpenseStatusEnum ExpenseStatusEnum { get; set; }
        public int EmployeeId { get; set; }
        public int ManagerId { get; set; }
        public int? AccountantId { get; set; }
        public ICollection<ExpenseResponse>? Expenses { get; set; }
    }

}
