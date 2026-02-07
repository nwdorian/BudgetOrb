namespace BudgetOrb.Application.Transactions.Contracts;

public record class GetTransactionByIdResponse(
    Guid Id,
    Guid CategoryId,
    decimal Amount,
    string? Comment,
    DateTime Date,
    string Category
);
