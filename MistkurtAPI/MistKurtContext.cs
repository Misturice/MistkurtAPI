
using Microsoft.EntityFrameworkCore;
using MistkurtAPI.Models;

namespace MistkurtAPI
{
    public class MistKurtContext : DbContext
    {
        public MistKurtContext(DbContextOptions < MistKurtContext > options): base(options) {}

        public DbSet<User> Users { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Expenses> Expenses { get; set; }
    }
}
