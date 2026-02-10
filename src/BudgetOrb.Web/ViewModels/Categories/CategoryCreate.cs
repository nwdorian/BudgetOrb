using System.ComponentModel.DataAnnotations;
using BudgetOrb.Application.Categories.Contracts;

namespace BudgetOrb.Web.ViewModels.Categories;

public class CategoryCreate
{
    [Required]
    [StringLength(100)]
    public string Name { get; set; } = string.Empty;
}

public static class CategoryCreateMapping
{
    extension(CategoryCreate model)
    {
        public CreateCategoryCommand ToCommand()
        {
            return new(model.Name);
        }
    }
}
