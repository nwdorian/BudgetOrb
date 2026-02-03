using BudgetOrb.Application.Abstractions;
using BudgetOrb.Application.Categories.Contracts;
using BudgetOrb.Application.Transactions.Contracts;
using BudgetOrb.Web.ViewModels.Transactions;
using BudgetOrb.Web.ViewModels.Transactions.Contracts;
using Microsoft.AspNetCore.Mvc;

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
}
