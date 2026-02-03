using System.ComponentModel.DataAnnotations;
using BudgetOrb.Application.Categories.Contracts;
using BudgetOrb.Application.Common.Enums;
using BudgetOrb.Application.Transactions.Contracts;
using BudgetOrb.Web.Constants;
using BudgetOrb.Web.ViewModels.Shared;
using BudgetOrb.Web.ViewModels.Transactions.Contracts;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BudgetOrb.Web.ViewModels.Transactions;

public class TransactionIndexViewModel
{
    public IReadOnlyList<TransactionItem> Transactions { get; set; } = [];
    public PagingMetadata PagingMetadata { get; set; } = default!;
    public SelectList Categories { get; set; } = default!;
    public SelectList SortColumns { get; set; } = default!;
    public SelectList SortOrders { get; set; } = default!;
    public SelectList PageSizes { get; set; } = default!;
    public string? Category { get; set; }
    public TransactionSortColumn SortColumn { get; set; }
    public SortOrder SortOrder { get; set; }
    public int PageSize { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public string? SearchTerm { get; set; }

    public class TransactionItem(Guid id, decimal amount, DateTime date, string category, string? comment)
    {
        public Guid Id { get; set; } = id;

        [DataType(DataType.Currency)]
        public decimal Amount { get; set; } = amount;

        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy H:mm}")]
        public DateTime Date { get; set; } = date;
        public string Category { get; set; } = category;
        public string? Comment { get; set; } = comment ?? "No comment";
    }

    public static TransactionIndexViewModel Create(
        GetTransactionPageRequest request,
        GetTransactionsPageResponse getTransactionsPageResponse,
        GetCategoriesResponse getCategoriesResponse
    )
    {
        var sortColumns = Enum.GetValues<TransactionSortColumn>()
            .Select(e => new
            {
                Value = e,
                Text = e switch
                {
                    TransactionSortColumn.Date => "Transaction date",
                    TransactionSortColumn.Category => "Category name",
                    _ => e.ToString(),
                },
            });

        return new TransactionIndexViewModel()
        {
            Transactions = getTransactionsPageResponse
                .Items.Select(t => new TransactionItem(t.Id, t.Amount, t.Date, t.Category, t.Comment))
                .ToList(),
            PagingMetadata = new PagingMetadata(
                getTransactionsPageResponse.PageNumber,
                getTransactionsPageResponse.PageSize,
                getTransactionsPageResponse.TotalCount,
                getTransactionsPageResponse.TotalPages,
                getTransactionsPageResponse.HasPreviousPage,
                getTransactionsPageResponse.HasNextPage
            ),
            SearchTerm = request.SearchTerm,
            Categories = new SelectList(
                getCategoriesResponse.Categories.Select(c => c.Name),
                selectedValue: request.Category
            ),
            SortColumns = new SelectList(sortColumns, "Value", "Text", selectedValue: request.SortColumn),
            SortOrders = new SelectList(Enum.GetValues<SortOrder>(), selectedValue: request.SortOrder),
            PageSizes = new SelectList(PagingDefaults.PageSizeOptions, selectedValue: request.PageSize),
            Category = request.Category,
            SortColumn = request.SortColumn,
            SortOrder = request.SortOrder,
            PageSize = request.PageSize,
            StartDate = request.StartDate,
            EndDate = request.EndDate,
        };
    }
}
