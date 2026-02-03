using BudgetOrb.Application.Abstractions;
using BudgetOrb.Application.Abstractions.Data;
using BudgetOrb.Application.Categories.Contracts;
using Microsoft.EntityFrameworkCore;

namespace BudgetOrb.Application.Categories;

public class CategoryService(IApplicationDbContext context) : ICategoryService
{
    public async Task<GetCategoriesResponse> Get(CancellationToken cancellationToken)
    {
        IReadOnlyList<GetCategoriesResponse.CategoryItem> categories = await context
            .Categories.AsNoTracking()
            .Select(c => new GetCategoriesResponse.CategoryItem(c.Id, c.Name))
            .ToListAsync(cancellationToken);

        return new GetCategoriesResponse(categories);
    }
}
