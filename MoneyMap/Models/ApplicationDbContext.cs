using Microsoft.EntityFrameworkCore;
using MoneyMap.Models.Entities;

namespace MoneyMap.Models
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options) { }

        public virtual DbSet<Users> Users { get; set; }
        public virtual DbSet<Categories> Categories { get; set; }

    }
}
