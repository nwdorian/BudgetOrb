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

    public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest();
        }

        Result<GetCategoryByIdResponse> getCategoryById = await categoryService.GetById(
            new GetCategoryByIdQuery(id),
            cancellationToken
        );

        if (getCategoryById.IsFailure)
        {
            ModelState.AddModelError(string.Empty, getCategoryById.Error.Description);
            return PartialView(Partials.DeleteCategory, new CategoryDelete());
        }

        return PartialView(Partials.DeleteCategory, CategoryDelete.Create(getCategoryById.Value));
    }

    [HttpPost, ActionName(nameof(Delete))]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(Guid id, CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest();
        }

        Result deleteCategory = await categoryService.Delete(new DeleteCategoryCommand(id), cancellationToken);

        if (deleteCategory.IsFailure)
        {
            ModelState.AddModelError(string.Empty, deleteCategory.Error.Description);
            return PartialView(Partials.DeleteCategory, new CategoryDelete());
        }

        return NoContent();
    }

    public async Task<IActionResult> Update(Guid id, CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest();
        }

        Result<GetCategoryByIdResponse> getCategoryById = await categoryService.GetById(
            new GetCategoryByIdQuery(id),
            cancellationToken
        );

        if (getCategoryById.IsFailure)
        {
            ModelState.AddModelError(string.Empty, getCategoryById.Error.Description);
            return PartialView(Partials.UpdateCategory, new CategoryUpdate());
        }

        return PartialView(Partials.UpdateCategory, CategoryUpdate.Create(getCategoryById.Value));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Update(Guid id, CategoryUpdate updateModel, CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
        {
            return PartialView(Partials.UpdateCategory, updateModel);
        }

        Result updateCategory = await categoryService.Update(id, updateModel.ToCommand(), cancellationToken);

        if (updateCategory.IsFailure)
        {
            ModelState.AddModelError(string.Empty, updateCategory.Error.Description);
            return PartialView(Partials.UpdateCategory, updateModel);
        }

        return NoContent();
    }
}
