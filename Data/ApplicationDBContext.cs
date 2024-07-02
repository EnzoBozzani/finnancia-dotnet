using Microsoft.EntityFrameworkCore;
using FinnanciaCSharp.Models;

namespace FinnanciaCSharp.Data
{
    public class ApplicationDBContext : DbContext
    {
        public ApplicationDBContext(DbContextOptions dbContextOptions) : base(dbContextOptions)
        {

        }
        public DbSet<Finance> Finances { get; set; }
        public DbSet<Sheet> Sheets { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<User> Users { get; set; }
    }
}