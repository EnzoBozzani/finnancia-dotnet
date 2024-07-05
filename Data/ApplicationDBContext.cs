using Microsoft.EntityFrameworkCore;
using FinnanciaCSharp.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace FinnanciaCSharp.Data
{
    public class ApplicationDBContext : IdentityDbContext<User>
    {
        public ApplicationDBContext(DbContextOptions dbContextOptions) : base(dbContextOptions)
        {

        }
        public DbSet<Finance> Finances { get; set; }
        public DbSet<Sheet> Sheets { get; set; }
        public DbSet<Category> Categories { get; set; }
    }
}