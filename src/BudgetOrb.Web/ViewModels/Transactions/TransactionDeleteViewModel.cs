using System.ComponentModel.DataAnnotations;

namespace BudgetOrb.Web.ViewModels.Transactions;

public class TransactionDeleteViewModel
{
    public Guid Id { get; set; }

    [DataType(DataType.DateTime)]
    [DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy H:mm}")]
    public DateTime Date { get; set; }

    [DataType(DataType.Currency)]
    public decimal Amount { get; set; }
    public string? Comment { get; set; }
    public required string Category { get; set; }

    public static TransactionDeleteViewModel Create(
        Guid id,
        DateTime date,
        decimal amount,
        string? comment,
        string category
    )
    {
        return new TransactionDeleteViewModel()
        {
            Id = id,
            Date = date,
            Amount = amount,
            Comment = comment ?? "No comment",
            Category = category,
        };
    }
}
