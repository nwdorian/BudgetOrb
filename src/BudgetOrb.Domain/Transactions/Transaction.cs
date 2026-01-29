using BudgetOrb.Domain.Categories;

namespace BudgetOrb.Domain.Transactions;

public class Transaction
{
    public Guid Id { get; set; }
    public Guid CategoryId { get; set; }
    public decimal Amount { get; set; }
    public string? Comment { get; set; }
    public DateTime Date { get; set; }
    public required Category Category { get; set; }
}
