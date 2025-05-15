using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MoneyMap.Models.Entities;

namespace MoneyMap.Models.Configurations
{
    public class CategoriesConfiguration : IEntityTypeConfiguration<Categories>
    {
        public void Configure(EntityTypeBuilder<Categories> builder)
        {
            builder.HasKey(p => p.IdCategory);
            builder.Property(p => p.DateRegistered).IsRequired().HasDefaultValueSql("GETDATE()");
            builder.Property(p => p.IsDeleted).HasDefaultValue(false).IsRequired();

            builder.Property(p => p.Title).IsRequired().HasMaxLength(100);
            builder.Property(p => p.IsInput).IsRequired();


        }
    }
}
