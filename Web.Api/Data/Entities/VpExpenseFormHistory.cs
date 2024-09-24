using Web.Api.Base.BaseEntities;
using Web.Api.Base.Enums;

namespace Web.Api.Data.Entities
{
    public class VpExpenseFormHistory 
    {
        public int Id { get; set; }
        public int ExpenseFormId { get; set; }
        public virtual VpExpenseForm ExpenseForm { get; set; }
        public string? MadeBy { get; set; }
        public HistoryActionEnum  Action{ get; set; }

        public DateTime Date { get; set; }


    }
}
