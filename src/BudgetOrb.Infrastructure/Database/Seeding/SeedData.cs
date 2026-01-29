using BudgetOrb.Domain.Categories;
using BudgetOrb.Domain.Transactions;
using BudgetOrb.Infrastructure.Database.Seeding.Fakers;
using Microsoft.EntityFrameworkCore;

namespace BudgetOrb.Infrastructure.Database.Seeding;

public class SeedData(ApplicationDbContext context)
{
    public async Task Run()
    {
        if (await context.Categories.AnyAsync() || await context.Transactions.AnyAsync())
        {
            return;
        }

        List<Category> categories = new CategoryFaker().Generate(CategoryFaker.CategoryCount);

        TransactionFaker transactionFaker = new();
        List<Transaction> transactions = new();
        int seed = 322;

        foreach (Category category in categories)
        {
            List<Transaction> batch = transactionFaker
                .UseSeed(seed++)
                .FinishWith(
                    (f, t) =>
                    {
                        t.CategoryId = category.Id;
                        t.Category = category;
                    }
                )
                .Generate(100);

            transactions.AddRange(batch);
        }

        context.Categories.AddRange(categories);
        context.Transactions.AddRange(transactions);
        await context.SaveChangesAsync();
    }
}
