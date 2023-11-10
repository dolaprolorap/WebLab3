using backend.Models.DB;
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
                    new User() { Id = Guid.NewGuid(), Name = "RadiantDwarf", Password = "1111"},
                    new User() { Id = Guid.NewGuid(), Name = "Dolaprolorap", Password = "2222"},
                    new User() { Id = Guid.NewGuid(), Name = "UltraGreed", Password = "3333"},
                    new User() { Id = Guid.NewGuid(), Name = "Reveqqq", Password = "4444"}
                );

            modelBuilder.HasPostgresExtension("uuid-ossp");
            modelBuilder.Entity<User>().HasIndex(u => u.Name).IsUnique(true);
        }
    }
}
