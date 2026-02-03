using BudgetOrb.Application.Transactions.Contracts;

namespace BudgetOrb.Application.Abstractions;

public interface ITransactionService
{
    Task<GetTransactionsPageResponse> GetPage(GetTransactionsPageQuery query, CancellationToken cancellationToken);
}
