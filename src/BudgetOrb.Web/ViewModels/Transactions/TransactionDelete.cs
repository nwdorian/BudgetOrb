using System.ComponentModel.DataAnnotations;
using BudgetOrb.Application.Transactions.Contracts;

namespace BudgetOrb.Web.ViewModels.Transactions;

public class TransactionDelete
{
    public Guid Id { get; set; }

    [DataType(DataType.DateTime)]
    [DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy H:mm}")]
    public DateTime Date { get; set; }

    [DataType(DataType.Currency)]
    public decimal Amount { get; set; }
    public string? Comment { get; set; }
    public required string Category { get; set; }
    public static TransactionDelete Empty =>
        new()
        {
            Id = Guid.Empty,
            Date = DateTime.Now,
            Amount = 0,
            Comment = string.Empty,
            Category = string.Empty,
        };

    public static TransactionDelete Create(GetTransactionByIdResponse getTransactionById)
    {
        return new TransactionDelete()
        {
            Id = getTransactionById.Id,
            Date = getTransactionById.Date,
            Amount = getTransactionById.Amount,
            Comment = getTransactionById.Comment ?? "No comment",
            Category = getTransactionById.Category,
        };
    }
}
