namespace BudgetOrb.Application.Categories.Contracts;

public record class GetCategoriesDetailsResponse(
    IReadOnlyList<GetCategoriesDetailsResponse.CategoryDetailsModel> Categories
)
{
    public record class CategoryDetailsModel(
        Guid Id,
        string Name,
        int TotalTransactions,
        decimal TotalTransactionsAmount
    );
}
