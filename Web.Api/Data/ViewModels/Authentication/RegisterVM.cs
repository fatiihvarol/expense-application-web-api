using Web.Api.Base.BaseEntities;
using Web.Api.Base.Enums;

namespace Web.Api.Data.ViewModels.Authentication
{
    public class RegisterVM : BaseEntity
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public UserRoleEnum Role { get; set; }
        public string? Name { get; set; }
        public string? Surname { get; set; }
        public DateTime DateofBirth { get; set; }
    }
}
