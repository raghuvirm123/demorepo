using LoginRegisterAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace LoginRegisterAPI.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options):base(options)
        {

        }

        public DbSet<User> Users { get; set; }
    }
}
