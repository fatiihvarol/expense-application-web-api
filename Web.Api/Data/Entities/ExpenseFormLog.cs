using Web.Api.Base.BaseEntities;

namespace Web.Api.Data.Entities
{
    public class ExpenseFormLog : VpBaseEntityWithId
    {
        public int ExpenseFormId { get; set; }
        public virtual VpExpenseForm? VpExpenseForm { get; set; }
        public string? Log { get; set; }
        
    }
}
