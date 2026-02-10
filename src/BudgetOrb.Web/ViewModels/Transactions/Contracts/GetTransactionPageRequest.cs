using BudgetOrb.Application.Common.Enums;
using BudgetOrb.Application.Transactions.Contracts;
using BudgetOrb.Web.Constants;

namespace BudgetOrb.Web.ViewModels.Transactions.Contracts;

public record class GetTransactionPageRequest(
    string? SearchTerm = null,
    string? Category = null,
    DateTime? StartDate = null,
    DateTime? EndDate = null,
    int PageNumber = PagingDefaults.PageNumber,
    int PageSize = PagingDefaults.PageSize,
    TransactionSortColumn SortColumn = TransactionSortColumn.Category,
    SortOrder SortOrder = SortOrder.Ascending
);

public static class GetTransactionPageMapping
{
    extension(GetTransactionPageRequest request)
    {
        public GetTransactionsPageQuery ToQuery()
        {
            return new(
                request.SearchTerm,
                request.Category,
                request.StartDate,
                request.StartDate,
                request.PageNumber,
                request.PageSize,
                request.SortColumn,
                request.SortOrder
            );
        }
    }
}
