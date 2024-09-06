using Web.Api.Base.BaseEntities;
using Web.Api.Base.Enums;

namespace Web.Api.Data.Entities
{
    public abstract class ApplicationUser : BaseEntityWithId
    {
        public string Email { get; set; }
        public string Password { get; set; }

        public UserRoleEnum Role { get; set; }
    }
}
