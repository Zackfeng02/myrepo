using Microsoft.EntityFrameworkCore;
using StreamingServiceApp.Models;

namespace StreamingServiceApp.DbData
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }
        public DbSet<User> Users { get; set; }


    }
}
