using Web.Api.Base.Enums;

namespace Web.Api.Schema
{
    public class EmployeeExpenseInfoResponse
    {
        public int PendingApprovalCount { get; set; }
        public int ApprovedCount { get; set; }
        public int TotalExpenseCount { get; set; }
        public Dictionary<CurrencyEnum, decimal> TotalExpenseAmount { get; set; } // Her para birimi için ayrı toplam
        public ICollection<ExpenseFormResponse>? LastExpenseForms { get; set; }
    }
}
