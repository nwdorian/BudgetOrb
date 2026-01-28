using BudgetOrb.Domain.Categories;
using BudgetOrb.Domain.Transactions;
using Microsoft.EntityFrameworkCore;

namespace BudgetOrb.Application.Abstractions.Data;

public interface IApplicationDbContext
{
    DbSet<Category> Categories { get; }
    DbSet<Transaction> Transactions { get; }
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
