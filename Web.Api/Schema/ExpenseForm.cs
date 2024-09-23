using Web.Api.Base.Enums;
using Web.Api.Data.Entities;

namespace Web.Api.Schema
{
    public class ExpenseFormRequest
    {
        public string? RejectionDescription { get; set; }

        public decimal TotalAmount { get; set; }

        public string? Currency { get; set; }
        public string? ExpenseStatus { get; set; }
        public ICollection<ExpenseRequest>? Expenses { get; set; }
    }
    public class ExpenseFormResponse
    {
        public int Id { get; set; }
        public string? RejectionDescription { get; set; }

        public decimal TotalAmount { get; set; }

        public string? Currency { get; set; }

        public string? ExpenseStatus { get; set; }
        public int EmployeeId { get; set; }
        public string? EmployeeFullName { get; set; }
        public int ManagerId { get; set; }
        public string? ManagerFullName { get; set; }
        public int? AccountantId { get; set; }
        public ICollection<ExpenseResponse>? Expenses { get; set; }
    }

}
