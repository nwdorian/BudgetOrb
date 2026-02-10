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

    public async Task<Result<GetCategoryByIdResponse>> GetById(
        GetCategoryByIdQuery query,
        CancellationToken cancellationToken
    )
    {
        GetCategoryByIdResponse? response = await context
            .Categories.Where(c => c.Id == query.Id)
            .Select(c => new GetCategoryByIdResponse(c.Id, c.Name))
            .FirstOrDefaultAsync(cancellationToken);

        if (response is null)
        {
            return CategoryErrors.NotFoundById(query.Id);
        }

        return response;
    }

    public async Task<Result> Create(CreateCategoryCommand command, CancellationToken cancellationToken)
    {
        bool nameExists = await context.Categories.AnyAsync(c => c.Name == command.Name, cancellationToken);

        if (nameExists)
        {
            return CategoryErrors.NameAlreadyExists(command.Name);
        }

        Category response = new() { Id = Guid.NewGuid(), Name = command.Name };

        context.Categories.Add(response);
        await context.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }

    public async Task<Result> Delete(DeleteCategoryCommand command, CancellationToken cancellationToken)
    {
        Category? response = await context.Categories.FirstOrDefaultAsync(c => c.Id == command.Id, cancellationToken);

        if (response is null)
        {
            return CategoryErrors.NotFoundById(command.Id);
        }

        context.Categories.Remove(response);
        await context.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
