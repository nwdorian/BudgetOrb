using BudgetOrb.Domain.Core.Results;

namespace BudgetOrb.Domain.Transactions;

public static class TransactionErrors
{
    public static Error NotFoundById(Guid id) =>
        Error.NotFound("Transaction.NotFoundById", $"The transaction with Id = {id} was not found.");
}
