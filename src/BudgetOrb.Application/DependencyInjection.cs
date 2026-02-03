using BudgetOrb.Application.Abstractions;
using BudgetOrb.Application.Categories;
using BudgetOrb.Application.Transactions;
using Microsoft.Extensions.DependencyInjection;

namespace BudgetOrb.Application;

public static class DependencyInjection
{
    public static void AddApplication(this IServiceCollection services)
    {
        services.AddScoped<ITransactionService, TransactionService>();
        services.AddScoped<ICategoryService, CategoryService>();
    }
}
