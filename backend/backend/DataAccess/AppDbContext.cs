using backend.Models;
using Microsoft.EntityFrameworkCore;

namespace backend.DataAccess
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Plot> Plots { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().HasData(
                new User { Id = 1, Name="RadiantDwarf", Password="1111" },
                new User { Id = 2, Name = "Dolaprolorap", Password = "2222" },
                new User { Id = 3, Name = "UltraGreed", Password = "3333" },
                new User { Id = 4, Name = "Reveqqq", Password = "4444" }
                );
        }
    }
}
