using System.Linq.Expressions;
using BudgetOrb.Application.Abstractions;
using BudgetOrb.Application.Abstractions.Data;
using BudgetOrb.Application.Common.Enums;
using BudgetOrb.Application.Transactions.Contracts;
using BudgetOrb.Domain.Core.Pagination;
using BudgetOrb.Domain.Transactions;
using Microsoft.EntityFrameworkCore;

namespace BudgetOrb.Application.Transactions;

public class TransactionService(IApplicationDbContext context) : ITransactionService
{
    public async Task<GetTransactionsPageResponse> GetPage(
        GetTransactionsPageQuery query,
        CancellationToken cancellationToken
    )
    {
        IQueryable<Transaction> transactionsQuery = context.Transactions.Include(t => t.Category).AsNoTracking();

        if (!string.IsNullOrWhiteSpace(query.SearchTerm))
        {
            transactionsQuery = transactionsQuery.Where(t => t.Comment != null && t.Comment.Contains(query.SearchTerm));
        }

        if (!string.IsNullOrWhiteSpace(query.Category))
        {
            transactionsQuery = transactionsQuery.Where(t => t.Category.Name == query.Category);
        }

        if (query.StartDate is not null)
        {
            transactionsQuery = transactionsQuery.Where(t => t.Date >= query.StartDate);
        }

        if (query.EndDate is not null)
        {
            transactionsQuery = transactionsQuery.Where(t => t.Date <= query.EndDate);
        }

        if (query.SortOrder == SortOrder.Descending)
        {
            transactionsQuery = transactionsQuery.OrderByDescending(GetSortColumn(query));
        }
        else
        {
            transactionsQuery = transactionsQuery.OrderBy(GetSortColumn(query));
        }

        int totalCount = await transactionsQuery.CountAsync(cancellationToken);

        GetTransactionsPageResponse.TransactionModel[] transactionsPage = await transactionsQuery
            .Skip((query.PageNumber - 1) * query.PageSize)
            .Take(query.PageSize)
            .Select(t => new GetTransactionsPageResponse.TransactionModel(
                t.Id,
                t.CategoryId,
                t.Amount,
                t.Comment,
                t.Date,
                t.Category.Name
            ))
            .ToArrayAsync(cancellationToken);

        PagedResult<GetTransactionsPageResponse.TransactionModel> pagedResult = new(
            transactionsPage,
            query.PageNumber,
            query.PageSize,
            totalCount
        );

        return new GetTransactionsPageResponse(
            pagedResult.PageNumber,
            pagedResult.PageSize,
            pagedResult.TotalCount,
            pagedResult.TotalPages,
            pagedResult.HasPreviousPage,
            pagedResult.HasNextPage,
            pagedResult.Items
        );
    }

    private static Expression<Func<Transaction, object>> GetSortColumn(GetTransactionsPageQuery query)
    {
        return query.SortColumn switch
        {
            TransactionSortColumn.Amount => transaction => transaction.Amount,
            TransactionSortColumn.Comment => transaction => transaction.Comment ?? "",
            TransactionSortColumn.Date => transaction => transaction.Date,
            TransactionSortColumn.Category => transaction => transaction.Category.Name,
            _ => transaction => transaction.Category.Name,
        };
    }
}
