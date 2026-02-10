using BudgetOrb.Application.Categories.Contracts;
using BudgetOrb.Domain.Core.Results;

namespace BudgetOrb.Application.Abstractions;

public interface ICategoryService
{
    Task<GetCategoriesResponse> Get(CancellationToken cancellationToken);
    Task<GetCategoriesDetailsResponse> GetDetails(CancellationToken cancellationToken);
    Task<Result> Create(CreateCategoryCommand command, CancellationToken cancellationToken);
}
