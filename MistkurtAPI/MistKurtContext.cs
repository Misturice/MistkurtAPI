
using Microsoft.EntityFrameworkCore;
using MistkurtAPI.Models;

namespace MistkurtAPI
{
    public class MistKurtContext : DbContext
    {
        public MistKurtContext(DbContextOptions < MistKurtContext > options): base(options) {}

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.HasPostgresExtension("uuid-ossp");
            builder.Entity<User>().HasIndex(p => new { p.Email }).IsUnique(true);
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Expenses> Expenses { get; set; }
    }
}
