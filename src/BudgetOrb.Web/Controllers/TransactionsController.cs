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
}
