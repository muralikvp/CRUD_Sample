using Microsoft.EntityFrameworkCore;

namespace WebApp1.Models
{
    public class HTContext : DbContext
    {
        public HTContext(DbContextOptions<HTContext> ob) : base(ob)
        {
        }
        public DbSet<User> user { get; set; } = default!;

        public DbSet<UserRoles> userRoles { get; set; } = default!;
    }
}
