using System.ComponentModel.DataAnnotations;
using BudgetOrb.Application.Categories.Contracts;
using BudgetOrb.Application.Transactions.Contracts;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BudgetOrb.Web.ViewModels.Transactions;

public class TransactionCreate
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

    public static TransactionCreate Create(GetCategoriesResponse getCategories)
    {
        return new TransactionCreate()
        {
            Categories = new SelectList(getCategories.Categories, "Id", "Name"),
            Date = DateTime.Now,
        };
    }

    public void SetCategories(GetCategoriesResponse getCategories)
    {
        Categories = new SelectList(getCategories.Categories, "Id", "Name");
    }
}

public static class TransactionCreateMapping
{
    extension(TransactionCreate model)
    {
        public CreateTransactionCommand ToCommand()
        {
            return new(model.CategoryId!.Value, model.Date!.Value, model.Amount, model.Comment);
        }
    }
}
