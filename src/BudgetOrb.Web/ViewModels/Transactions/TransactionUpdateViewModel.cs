using System.ComponentModel.DataAnnotations;
using BudgetOrb.Application.Categories.Contracts;
using BudgetOrb.Application.Transactions.Contracts;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BudgetOrb.Web.ViewModels.Transactions;

public class TransactionUpdateViewModel
{
    public SelectList? Categories { get; set; }

    [Required(ErrorMessage = "Please select a category")]
    [Display(Name = "Category")]
    public Guid? CategoryId { get; set; }

    [Required]
    [DataType(DataType.DateTime)]
    [DisplayFormat(DataFormatString = "{0:yyyy-MM-ddTHH:mm}", ApplyFormatInEditMode = true)]
    public DateTime? Date { get; set; }

    [Required]
    [Range(0.01, double.MaxValue, ErrorMessage = "Amount must be greater than zero")]
    public decimal Amount { get; set; }

    [MaxLength(255)]
    public string? Comment { get; set; }

    public static TransactionUpdateViewModel Empty =>
        new()
        {
            Categories = new SelectList(new List<SelectListItem>()),
            CategoryId = Guid.Empty,
            Date = DateTime.Now,
            Amount = 0,
            Comment = string.Empty,
        };

    public static TransactionUpdateViewModel Create(
        GetTransactionByIdResponse getTransactionByIdResponse,
        GetCategoriesResponse getCategoriesResponse
    )
    {
        return new TransactionUpdateViewModel()
        {
            Categories = new SelectList(
                getCategoriesResponse.Categories,
                "Id",
                "Name",
                selectedValue: getTransactionByIdResponse.CategoryId
            ),
            CategoryId = getTransactionByIdResponse.CategoryId,
            Date = getTransactionByIdResponse.Date,
            Amount = getTransactionByIdResponse.Amount,
            Comment = getTransactionByIdResponse.Comment,
        };
    }
}

public static class TransactionUpdateMapping
{
    extension(TransactionUpdateViewModel model)
    {
        public UpdateTransactionCommand ToCommand()
        {
            return new(model.CategoryId!.Value, model.Amount, model.Date!.Value, model.Comment);
        }
    }
}
