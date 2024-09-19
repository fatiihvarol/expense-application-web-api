using Web.Api.Base.BaseEntities;
using Web.Api.Base.Enums;

namespace Web.Api.Data.Entities
{
    public abstract class VpApplicationUser : VpBaseEntityWithId
    {
        public string? Email { get; set; }
        public string? Password { get; set; }

        public UserRoleEnum Role { get; set; }

        public string? Name { get; set; }
        public string? Surname { get; set; }
        public DateTime DateofBirth { get; set; }
    }
}
