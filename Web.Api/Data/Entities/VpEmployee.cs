namespace Web.Api.Data.Entities
{
    public class VpEmployee : VpApplicationUser
    {
        public int ManagerId { get; set; }
        public VpManager? Manager { get; set; }

        public ICollection<VpExpenseForm>?VpExpenseForms { get; set; }


    }
}
