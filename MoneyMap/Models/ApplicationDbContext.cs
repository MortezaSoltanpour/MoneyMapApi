using Microsoft.EntityFrameworkCore;
using MoneyMap.Models.Entities;
using System.Linq.Expressions;
using System.Reflection;

namespace MoneyMap.Models
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options) { }

        public virtual DbSet<Users> Users { get; set; }
        public virtual DbSet<Categories> Categories { get; set; }
        public virtual DbSet<Transactions> Transactions { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

            // Apply a global query filter for entities with an IsDeleted field
            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                var isDeletedProperty = entityType.FindProperty("IsDeleted");
                if (isDeletedProperty != null && isDeletedProperty.ClrType == typeof(bool))
                {
                    var parameter = Expression.Parameter(entityType.ClrType, "e");
                    var body = Expression.Equal(
                        Expression.Property(parameter, "IsDeleted"),
                        Expression.Constant(false)
                    );

                    var lambda = Expression.Lambda(body, parameter);
                    modelBuilder.Entity(entityType.ClrType).HasQueryFilter(lambda);
                }
            }
        }
    }
}
