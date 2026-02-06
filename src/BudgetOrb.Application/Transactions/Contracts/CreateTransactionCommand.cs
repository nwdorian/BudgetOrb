namespace BudgetOrb.Application.Transactions.Contracts;

public record class CreateTransactionCommand(Guid CategoryId, DateTime Date, decimal Amount, string? Comment);
