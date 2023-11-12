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
        public DbSet<PlotEntry> Entries { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasPostgresExtension("uuid-ossp");
            modelBuilder.Entity<User>().HasIndex(u => u.UserName).IsUnique(true);
        }
    }
}
