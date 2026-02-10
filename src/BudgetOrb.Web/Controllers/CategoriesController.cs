using BudgetOrb.Application.Abstractions;
using BudgetOrb.Application.Categories.Contracts;
using BudgetOrb.Domain.Core.Results;
using BudgetOrb.Web.Constants;
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

    public async Task<IActionResult> Create()
    {
        return PartialView(Partials.CreateCategory, new CategoryCreate());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(CategoryCreate createModel, CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
        {
            return PartialView(Partials.CreateCategory, createModel);
        }

        Result createCategory = await categoryService.Create(createModel.ToCommand(), cancellationToken);

        if (createCategory.IsFailure)
        {
            ModelState.AddModelError(string.Empty, createCategory.Error.Description);
            return PartialView(Partials.CreateCategory, createModel);
        }

        return Created();
    }
}
