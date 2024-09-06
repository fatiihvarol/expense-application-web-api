using Web.Api.Data.Entities;

namespace Web.Api.Data.Entities
{
    public class User : ApplicationUser
    {
        public string? Name { get; set; }
        public string? Surname { get; set; }

        public DateTime DateofBirth { get; set; }


    }
}
