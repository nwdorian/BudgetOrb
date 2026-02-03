namespace BudgetOrb.Application.Transactions.Contracts;

public record class GetTransactionsPageResponse(
    int PageNumber,
    int PageSize,
    int TotalCount,
    int TotalPages,
    bool HasPreviousPage,
    bool HasNextPage,
    IReadOnlyList<GetTransactionsPageResponse.TransactionModel> Items
)
{
    public record class TransactionModel(
        Guid Id,
        Guid CategoryId,
        decimal Amount,
        string? Comment,
        DateTime Date,
        string Category
    );
}
