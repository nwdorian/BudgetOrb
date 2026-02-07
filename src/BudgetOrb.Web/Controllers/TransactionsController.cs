using BudgetOrb.Application.Abstractions;
using BudgetOrb.Application.Categories.Contracts;
using BudgetOrb.Application.Transactions.Contracts;
using BudgetOrb.Domain.Core.Results;
using BudgetOrb.Web.ViewModels.Transactions;
using BudgetOrb.Web.ViewModels.Transactions.Contracts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BudgetOrb.Web.Controllers;

public class TransactionsController(ITransactionService transactionService, ICategoryService categoryService)
    : Controller
{
    public async Task<IActionResult> Index(
        [FromQuery] GetTransactionPageRequest request,
        CancellationToken cancellationToken = default
    )
    {
        if (!ModelState.IsValid)
        {
            return BadRequest();
        }

        GetTransactionsPageQuery getTransactionsPageQuery = new(
            request.SearchTerm,
            request.Category,
            request.StartDateUtc,
            request.EndDateUtc,
            request.PageNumber,
            request.PageSize,
            request.SortColumn,
            request.SortOrder
        );

        GetTransactionsPageResponse getTransactionsPageResponse = await transactionService.GetPage(
            getTransactionsPageQuery,
            cancellationToken
        );

        GetCategoriesResponse getCategoriesResponse = await categoryService.Get(cancellationToken);

        return View(TransactionIndexViewModel.Create(request, getTransactionsPageResponse, getCategoriesResponse));
    }

    public async Task<IActionResult> Create(CancellationToken cancellationToken = default)
    {
        GetCategoriesResponse getCategoriesResponse = await categoryService.Get(cancellationToken);

        return PartialView("_CreateTransactionPartial", TransactionCreateViewModel.Create(getCategoriesResponse));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(
        TransactionCreateViewModel createModel,
        CancellationToken cancellationToken = default
    )
    {
        if (!ModelState.IsValid)
        {
            GetCategoriesResponse getCategoriesResponse = await categoryService.Get(cancellationToken);
            createModel.Categories = new SelectList(getCategoriesResponse.Categories, "Id", "Name");

            return PartialView("_CreateTransactionPartial", createModel);
        }

        CreateTransactionCommand createTransactionCommand = new(
            createModel.CategoryId!.Value,
            DateTime.SpecifyKind(createModel.Date!.Value, DateTimeKind.Utc),
            createModel.Amount,
            createModel.Comment
        );

        Result createTransaction = await transactionService.Create(createTransactionCommand, cancellationToken);

        if (createTransaction.IsFailure)
        {
            ModelState.AddModelError(string.Empty, createTransaction.Error.Description);

            GetCategoriesResponse getCategoriesResponse = await categoryService.Get(cancellationToken);
            createModel.Categories = new SelectList(getCategoriesResponse.Categories, "Id", "Name");

            return PartialView("_CreateTransactionPartial", createModel);
        }

        return Created();
    }

    public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken = default)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest();
        }

        GetTransactionByIdQuery getTransactionByIdQuery = new(id);
        Result<GetTransactionByIdResponse> getTransactionByIdResponse = await transactionService.GetById(
            getTransactionByIdQuery,
            cancellationToken
        );

        if (getTransactionByIdResponse.IsFailure)
        {
            ModelState.AddModelError(string.Empty, getTransactionByIdResponse.Error.Description);

            return PartialView(
                "_DeleteTransactionPartial",
                TransactionDeleteViewModel.Create(Guid.Empty, DateTime.Now, 0, string.Empty, string.Empty)
            );
        }

        return PartialView(
            "_DeleteTransactionPartial",
            TransactionDeleteViewModel.Create(
                getTransactionByIdResponse.Value.Id,
                getTransactionByIdResponse.Value.Date,
                getTransactionByIdResponse.Value.Amount,
                getTransactionByIdResponse.Value.Comment,
                getTransactionByIdResponse.Value.Category
            )
        );
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(Guid id, CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest();
        }

        DeleteTransactionCommand deleteTransactionCommand = new(id);
        Result deleteTransactionResult = await transactionService.Delete(deleteTransactionCommand, cancellationToken);

        if (deleteTransactionResult.IsFailure)
        {
            ModelState.AddModelError(string.Empty, deleteTransactionResult.Error.Description);

            return PartialView(
                "_DeleteTransactionPartial",
                TransactionDeleteViewModel.Create(Guid.Empty, DateTime.Now, 0, string.Empty, string.Empty)
            );
        }

        return NoContent();
    }
}
