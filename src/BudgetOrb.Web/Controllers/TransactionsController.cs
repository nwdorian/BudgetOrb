using BudgetOrb.Application.Abstractions;
using BudgetOrb.Application.Categories.Contracts;
using BudgetOrb.Application.Transactions.Contracts;
using BudgetOrb.Domain.Core.Results;
using BudgetOrb.Web.Constants;
using BudgetOrb.Web.ViewModels.Transactions;
using BudgetOrb.Web.ViewModels.Transactions.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace BudgetOrb.Web.Controllers;

public class TransactionsController(ITransactionService transactionService, ICategoryService categoryService)
    : Controller
{
    public async Task<IActionResult> Index(
        [FromQuery] GetTransactionPageRequest request,
        CancellationToken cancellationToken
    )
    {
        if (!ModelState.IsValid)
        {
            return BadRequest();
        }

        GetTransactionsPageResponse getTransactionsPage = await transactionService.GetPage(
            request.ToQuery(),
            cancellationToken
        );

        GetCategoriesResponse getCategories = await categoryService.Get(cancellationToken);

        return View(TransactionIndex.Create(request, getTransactionsPage, getCategories));
    }

    public async Task<IActionResult> Create(CancellationToken cancellationToken = default)
    {
        return PartialView(
            Partials.CreateTransaction,
            TransactionCreate.Create(await categoryService.Get(cancellationToken))
        );
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(TransactionCreate createModel, CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
        {
            createModel.SetCategories(await categoryService.Get(cancellationToken));
            return PartialView(Partials.CreateTransaction, createModel);
        }

        Result createTransaction = await transactionService.Create(createModel.ToCommand(), cancellationToken);

        if (createTransaction.IsFailure)
        {
            ModelState.AddModelError(string.Empty, createTransaction.Error.Description);
            createModel.SetCategories(await categoryService.Get(cancellationToken));
            return PartialView(Partials.CreateTransaction, createModel);
        }

        return Created();
    }

    public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest();
        }

        Result<GetTransactionByIdResponse> getTransactionById = await transactionService.GetById(
            new GetTransactionByIdQuery(id),
            cancellationToken
        );

        if (getTransactionById.IsFailure)
        {
            ModelState.AddModelError(string.Empty, getTransactionById.Error.Description);
            return PartialView(Partials.DeleteTransaction, TransactionDelete.Empty);
        }

        return PartialView(Partials.DeleteTransaction, TransactionDelete.Create(getTransactionById.Value));
    }

    [HttpPost, ActionName(nameof(Delete))]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(Guid id, CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest();
        }

        Result deleteTransaction = await transactionService.Delete(new DeleteTransactionCommand(id), cancellationToken);

        if (deleteTransaction.IsFailure)
        {
            ModelState.AddModelError(string.Empty, deleteTransaction.Error.Description);
            return PartialView(Partials.DeleteTransaction, TransactionDelete.Empty);
        }

        return NoContent();
    }

    public async Task<IActionResult> Update(Guid id, CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest();
        }

        Result<GetTransactionByIdResponse> getTransactionById = await transactionService.GetById(
            new GetTransactionByIdQuery(id),
            cancellationToken
        );

        if (getTransactionById.IsFailure)
        {
            ModelState.AddModelError(string.Empty, getTransactionById.Error.Description);
            return PartialView(Partials.UpdateTransaction, TransactionUpdate.Empty);
        }

        GetCategoriesResponse getCategories = await categoryService.Get(cancellationToken);

        return PartialView(
            Partials.UpdateTransaction,
            TransactionUpdate.Create(getTransactionById.Value, getCategories)
        );
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Update(Guid id, TransactionUpdate updateModel, CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
        {
            updateModel.SetCategories(await categoryService.Get(cancellationToken));
            return PartialView(Partials.UpdateTransaction, updateModel);
        }

        Result updateTransaction = await transactionService.Update(id, updateModel.ToCommand(), cancellationToken);

        if (updateTransaction.IsFailure)
        {
            ModelState.AddModelError(string.Empty, updateTransaction.Error.Description);
            updateModel.SetCategories(await categoryService.Get(cancellationToken));
            return PartialView(Partials.UpdateTransaction, updateModel);
        }

        return NoContent();
    }
}
