using backend.Models.DB;
using Microsoft.EntityFrameworkCore;
using System;

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
                    new User (guid : new Guid("32e8110c-b664-4a8b-b69d-f59fba269314"), name : "RadiantDwarf", password: "1111"),
                    new User (guid : new Guid("3b078e3f-e9a4-4098-adf2-ced840603bb4"), name : "Dolaprolorap", password: "2222"),
                    new User (guid : new Guid("5399ee18-ffdf-470b-bbae-160287b33244"), name : "UltraGreed", password: "3333"),
                    new User (guid : new Guid("da50fb97-6cca-4b03-af03-2d34ef433d59"), name : "Reveqqq", password: "4444")
                );

            modelBuilder.HasPostgresExtension("uuid-ossp");
            modelBuilder.Entity<User>().HasIndex(u => u.UserName).IsUnique(true);
        }
    }
}
