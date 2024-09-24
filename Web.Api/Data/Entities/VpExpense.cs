using System.ComponentModel.DataAnnotations;
using Web.Api.Base.BaseEntities;

namespace Web.Api.Data.Entities
{
    public class VpExpense : VpBaseEntityWithId
    {
        [Range(0, 5000, ErrorMessage = "Amount cannot exceed 5000.")]
        public decimal Amount { get; set; }
        public string? Description { get; set; }
        public string? Location { get; set; }
        public int CategoryId { get; set; }
        public VpExpenseCategory Category { get; set; }
        public string? ReceiptNumber { get; set; }

        public int ExpenseFormId { get; set; }
        public virtual VpExpenseForm? VpExpenseForm { get; set; }
    }
}
