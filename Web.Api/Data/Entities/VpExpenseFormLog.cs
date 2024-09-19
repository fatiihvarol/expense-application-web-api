using Web.Api.Base.BaseEntities;
using Web.Api.Base.Enums;

namespace Web.Api.Data.Entities
{
    public class VpExpenseFormLog 
    {
        public int Id { get; set; }
        public int ExpenseFormId { get; set; }
        public decimal? OldTotalAmount { get; set; }
        public decimal? NewTotalAmount { get; set; }
        public CurrencyEnum? OldCurrency { get; set; }
        public CurrencyEnum? NewCurrency { get; set; }
        public ExpenseStatusEnum? OldExpenseStatus { get; set; }
        public ExpenseStatusEnum? NewExpenseStatus { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string? ModifiedBy { get; set; }

        public virtual VpExpenseForm? VpExpenseForm { get; set; }


    }
}
