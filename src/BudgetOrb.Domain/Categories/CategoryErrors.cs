using BudgetOrb.Domain.Core.Results;

namespace BudgetOrb.Domain.Categories;

public static class CategoryErrors
{
    public static Error NotFoundById(Guid id) =>
        Error.NotFound("Category.NotFoundById", $"The category with Id = {id} was not found.");
}
