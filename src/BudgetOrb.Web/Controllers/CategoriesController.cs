using BudgetOrb.Application.Abstractions;
using BudgetOrb.Application.Categories.Contracts;
using BudgetOrb.Web.ViewModels.Categories;
using Microsoft.AspNetCore.Mvc;

namespace BudgetOrb.Web.Controllers;

public class CategoriesController(ICategoryService categoryService) : Controller
{
    public async Task<IActionResult> Index(CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest();
        }

        GetCategoriesDetailsResponse getCategoriesDetails = await categoryService.GetDetails(cancellationToken);

        return View(CategoryIndex.Create(getCategoriesDetails));
    }
}
