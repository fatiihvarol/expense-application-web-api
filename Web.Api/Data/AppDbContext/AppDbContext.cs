using Microsoft.EntityFrameworkCore;
using Web.Api.Data.Entities;

namespace Web.Api.Data.AppDbContext
{
    public class AppDbContext : DbContext
    {

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
    }
}
