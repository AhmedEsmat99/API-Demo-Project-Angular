using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Api.Model
{
    public class Context : IdentityDbContext<ApplicationUser>
    {
        public Context() : base()
        {

        }
        public Context(DbContextOptions options) : base(options)
        {

        }
        public DbSet<Employee> Employees { get; set; }

    }
}
