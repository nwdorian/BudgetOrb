using BudgetOrb.Application.Categories.Contracts;
using BudgetOrb.Domain.Core.Results;

namespace BudgetOrb.Application.Abstractions;

public interface ICategoryService
{
    Task<GetCategoriesResponse> Get(CancellationToken cancellationToken);
    Task<GetCategoriesDetailsResponse> GetDetails(CancellationToken cancellationToken);
    Task<Result<GetCategoryByIdResponse>> GetById(GetCategoryByIdQuery query, CancellationToken cancellationToken);
    Task<Result> Create(CreateCategoryCommand command, CancellationToken cancellationToken);
    Task<Result> Delete(DeleteCategoryCommand command, CancellationToken cancellationToken);
    Task<Result> Update(Guid id, UpdateCategoryCommand command, CancellationToken cancellationToken);
}
