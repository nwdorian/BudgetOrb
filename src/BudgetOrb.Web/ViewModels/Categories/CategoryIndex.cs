using System.ComponentModel.DataAnnotations;
using BudgetOrb.Application.Categories.Contracts;

namespace BudgetOrb.Web.ViewModels.Categories;

public class CategoryIndex
{
    public IReadOnlyList<CategoryItem> Categories { get; set; } = [];

    public class CategoryItem(Guid id, string name, int totalTransactions, decimal totalTransactionsAmount)
    {
        public Guid Id { get; set; } = id;
        public string Name { get; set; } = name;

        [Display(Name = "Transactions")]
        public int TotalTransactions { get; set; } = totalTransactions;

        [DataType(DataType.Currency)]
        [Display(Name = "Total amount")]
        public decimal TotalTransactionsAmount { get; set; } = totalTransactionsAmount;
    }

    public static CategoryIndex Create(GetCategoriesDetailsResponse getCategoriesDetails)
    {
        return new CategoryIndex()
        {
            Categories = getCategoriesDetails
                .Categories.Select(c => new CategoryItem(c.Id, c.Name, c.TotalTransactions, c.TotalTransactionsAmount))
                .ToList(),
        };
    }
}
