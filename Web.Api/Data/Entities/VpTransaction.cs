using Web.Api.Base.BaseEntities;
using Web.Api.Base.Enums;

namespace Web.Api.Data.Entities
{
    public class VpTransaction : VpBaseEntityWithId
    {
        public decimal TotalAmount { get; set; }
        public string? IbanNumber { get; set; }
        public CurrencyEnum CurrencyEnum { get; set; }
        public int ExpenseFormId { get; set; }
        public virtual VpExpenseForm? ExpenseForm { get; set; }
        public string? Comments { get; set; }

    }
}
