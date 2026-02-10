using BudgetOrb.Application.Categories.Contracts;

namespace BudgetOrb.Web.ViewModels.Categories;

public class CategoryDelete
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;

    public static CategoryDelete Create(GetCategoryByIdResponse getCategoryById)
    {
        return new() { Id = getCategoryById.Id, Name = getCategoryById.Name };
    }
}
