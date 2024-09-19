using Web.Api.Base.BaseEntities;
using Web.Api.Base.Enums;

namespace Web.Api.Data.Entities
{
    public class VpExpenseForm : VpBaseEntityWithId
    {

        public string? RejectionDescription { get; set; }

        public decimal TotalAmount { get; set; }

        public CurrencyEnum CurrencyEnum { get; set; }

        public ExpenseStatusEnum ExpenseStatusEnum { get; set; }

        public ICollection<VpExpense>? Expenses { get; set; }

        public int EmployeeId { get; set; }
        public virtual VpEmployee? VpEmployee { get; set; }

        public int ManagerId { get; set; }
        public virtual VpManager? VpManager { get; set; }

        public int? AccountantId { get; set; }
        public virtual VpAccountant? VpAccountant { get; set; }
    }
}
