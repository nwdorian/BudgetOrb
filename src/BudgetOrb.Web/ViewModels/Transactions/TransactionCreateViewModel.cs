using System.ComponentModel.DataAnnotations;
using BudgetOrb.Application.Categories.Contracts;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BudgetOrb.Web.ViewModels.Transactions;

public class TransactionCreateViewModel
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

    public static TransactionCreateViewModel Create(GetCategoriesResponse getCategoriesResponse)
    {
        return new TransactionCreateViewModel()
        {
            Categories = new SelectList(getCategoriesResponse.Categories, "Id", "Name"),
            Date = DateTime.Now,
        };
    }
}
