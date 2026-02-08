using BudgetOrb.Application.Transactions.Contracts;
using BudgetOrb.Domain.Core.Results;

namespace BudgetOrb.Application.Abstractions;

public interface ITransactionService
{
    Task<GetTransactionsPageResponse> GetPage(GetTransactionsPageQuery query, CancellationToken cancellationToken);
    Task<Result<GetTransactionByIdResponse>> GetById(
        GetTransactionByIdQuery query,
        CancellationToken cancellationToken
    );
    Task<Result> Create(CreateTransactionCommand command, CancellationToken cancellationToken);
    Task<Result> Delete(DeleteTransactionCommand command, CancellationToken cancellationToken);
    Task<Result> Update(Guid id, UpdateTransactionCommand command, CancellationToken cancellationToken);
}
