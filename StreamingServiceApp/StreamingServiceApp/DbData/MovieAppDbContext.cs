using StreamingServiceApp.Models;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace StreamingServiceApp.DbData
{
    public class MovieAppDbContext : DbContext
    {
        public MovieAppDbContext(DbContextOptions<MovieAppDbContext> options)
            : base(options)
        {
        }
        public DbSet<Movie> Movies { get; set; }
        public DbSet<User> Users { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (optionsBuilder.IsConfigured)
            {
                return;
            }
            optionsBuilder.UseSqlServer($"");
            base.OnConfiguring(optionsBuilder);
        }

    }
}
