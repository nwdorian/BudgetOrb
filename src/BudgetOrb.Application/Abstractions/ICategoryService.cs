using BudgetOrb.Application.Categories.Contracts;

namespace BudgetOrb.Application.Abstractions;

public interface ICategoryService
{
    Task<GetCategoriesResponse> Get(CancellationToken cancellationToken);
}
