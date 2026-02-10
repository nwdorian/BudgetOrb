using BudgetOrb.Application.Abstractions;
using BudgetOrb.Application.Abstractions.Data;
using BudgetOrb.Application.Categories.Contracts;
using BudgetOrb.Domain.Categories;
using BudgetOrb.Domain.Core.Results;
using Microsoft.EntityFrameworkCore;

namespace BudgetOrb.Application.Categories;

public class CategoryService(IApplicationDbContext context) : ICategoryService
{
    public async Task<GetCategoriesResponse> Get(CancellationToken cancellationToken)
    {
        IReadOnlyList<GetCategoriesResponse.CategoryItem> categories = await context
            .Categories.Select(c => new GetCategoriesResponse.CategoryItem(c.Id, c.Name))
            .ToListAsync(cancellationToken);

        return new GetCategoriesResponse(categories);
    }

    public async Task<GetCategoriesDetailsResponse> GetDetails(CancellationToken cancellationToken)
    {
        IReadOnlyList<GetCategoriesDetailsResponse.CategoryDetailsModel> categories = await context
            .Categories.OrderBy(c => c.Name)
            .Select(c => new GetCategoriesDetailsResponse.CategoryDetailsModel(
                c.Id,
                c.Name,
                c.Transactions.Count,
                c.Transactions.Sum(t => t.Amount)
            ))
            .ToListAsync(cancellationToken);

        return new GetCategoriesDetailsResponse(categories);
    }

    public async Task<Result> Create(CreateCategoryCommand command, CancellationToken cancellationToken)
    {
        bool nameExists = await context.Categories.AnyAsync(c => c.Name == command.Name, cancellationToken);

        if (nameExists)
        {
            return CategoryErrors.NameAlreadyExists(command.Name);
        }

        Category category = new() { Id = Guid.NewGuid(), Name = command.Name };

        context.Categories.Add(category);
        await context.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
