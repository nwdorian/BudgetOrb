using BudgetOrb.Application.Common.Enums;

namespace BudgetOrb.Application.Transactions.Contracts;

public record class GetTransactionsPageQuery(
    string? SearchTerm,
    string? Category,
    DateTime? StartDate,
    DateTime? EndDate,
    int PageNumber,
    int PageSize,
    TransactionSortColumn SortColumn,
    SortOrder SortOrder
);
