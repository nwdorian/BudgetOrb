namespace BudgetOrb.Application.Categories.Contracts;

public record class GetCategoriesResponse(IReadOnlyList<GetCategoriesResponse.CategoryItem> Categories)
{
    public record class CategoryItem(Guid Id, string Name);
}
