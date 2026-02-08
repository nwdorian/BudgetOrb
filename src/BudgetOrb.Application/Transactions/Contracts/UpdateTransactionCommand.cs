namespace BudgetOrb.Application.Transactions.Contracts;

public record class UpdateTransactionCommand(Guid CategoryId, decimal Amount, DateTime Date, string? Comment);
