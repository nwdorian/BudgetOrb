using System.ComponentModel.DataAnnotations;
using BudgetOrb.Application.Categories.Contracts;

namespace BudgetOrb.Web.ViewModels.Categories;

public class CategoryUpdate
{
    public Guid? Id { get; set; }

    [Required]
    [StringLength(100)]
    public string Name { get; set; } = string.Empty;

    public static CategoryUpdate Create(GetCategoryByIdResponse getCategoryById)
    {
        return new() { Id = getCategoryById.Id, Name = getCategoryById.Name };
    }
}

public static class CategoryUpdateMapping
{
    extension(CategoryUpdate model)
    {
        public UpdateCategoryCommand ToCommand()
        {
            return new(model.Name);
        }
    }
}
