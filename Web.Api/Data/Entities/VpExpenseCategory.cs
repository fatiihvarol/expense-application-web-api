using System.ComponentModel.DataAnnotations;
using Web.Api.Base.BaseEntities;

namespace Web.Api.Data.Entities
{
    public class VpExpenseCategory : VpBaseEntity

    {
        [Key]
        public int CategoryId { get; set; }
        public string? CategoryName { get; set; }
        public string? Description { get; set; }



    }
}
