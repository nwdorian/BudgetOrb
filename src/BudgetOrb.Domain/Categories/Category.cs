using BudgetOrb.Domain.Transactions;

namespace BudgetOrb.Domain.Categories;

public class Category
{
    public Guid Id { get; set; }
    public required string Name { get; set; }
    public ICollection<Transaction> Transactions { get; init; } = [];
}
