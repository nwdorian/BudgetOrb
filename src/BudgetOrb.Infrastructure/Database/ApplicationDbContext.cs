using BudgetOrb.Application.Abstractions.Data;
using BudgetOrb.Domain.Categories;
using BudgetOrb.Domain.Transactions;
using Microsoft.EntityFrameworkCore;

namespace BudgetOrb.Infrastructure.Database;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
    : DbContext(options),
        IApplicationDbContext
{
    public DbSet<Category> Categories { get; set; }
    public DbSet<Transaction> Transactions { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
    }
}
