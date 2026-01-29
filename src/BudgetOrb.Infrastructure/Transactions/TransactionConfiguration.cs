using BudgetOrb.Domain.Transactions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BudgetOrb.Infrastructure.Transactions;

public class TransactionConfiguration : IEntityTypeConfiguration<Transaction>
{
    public void Configure(EntityTypeBuilder<Transaction> builder)
    {
        builder.HasKey(t => t.Id);

        builder.Property(t => t.Amount).HasPrecision(18, 2);

        builder.Property(t => t.Comment).HasMaxLength(255);

        builder.HasOne(t => t.Category).WithMany(t => t.Transactions).HasForeignKey(t => t.CategoryId);
    }
}
