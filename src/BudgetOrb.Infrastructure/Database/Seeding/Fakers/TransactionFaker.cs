using Bogus;
using BudgetOrb.Domain.Transactions;

namespace BudgetOrb.Infrastructure.Database.Seeding.Fakers;

public sealed class TransactionFaker : Faker<Transaction>
{
    public TransactionFaker()
    {
        RuleFor(x => x.Id, f => f.Random.Guid());
        RuleFor(x => x.Amount, f => f.Finance.Amount(5, 1000));
        RuleFor(x => x.Comment, f => f.Lorem.Sentence().OrNull(f, .3f));
        RuleFor(x => x.Date, f => f.Date.Recent(100));
    }
}
