using Web.Api.Base.Enums;

namespace Web.Api.Data.Entities
{
    public class VpManager : VpApplicationUser
    {
        public ICollection<VpEmployee>? Employees { get; set; }
    }
}
