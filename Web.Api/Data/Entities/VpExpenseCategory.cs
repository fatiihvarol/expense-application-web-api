using Web.Api.Base.BaseEntities;

namespace Web.Api.Data.Entities
{
    public class VpExpenseCategory : VpBaseEntityWithId
    {
        public string? CategoryName { get; set; }
        public string? Description { get; set; }



    }
}
