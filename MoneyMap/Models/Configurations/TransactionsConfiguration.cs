using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MoneyMap.Models.Entities;

namespace MoneyMap.Models.Configurations
{
    public class TransactionsConfiguration : IEntityTypeConfiguration<Transactions>
    {
        public void Configure(EntityTypeBuilder<Transactions> builder)
        {
            builder.HasKey(p => p.IdTransaction);
            builder.Property(p => p.DateRegistered).IsRequired().HasDefaultValueSql("GETDATE()");
            builder.Property(p => p.IsDeleted).HasDefaultValue(false).IsRequired();

            builder.Property(p => p.Description).IsRequired().HasMaxLength(500);
            builder.Property(p => p.Amount).IsRequired();
            builder.Property(p => p.FileAttached).HasMaxLength(50);
            builder.Property(p => p.CategoryId).IsRequired();
        }
    }
}
