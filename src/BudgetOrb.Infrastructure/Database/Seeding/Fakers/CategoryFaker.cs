using Bogus;
using BudgetOrb.Domain.Categories;

namespace BudgetOrb.Infrastructure.Database.Seeding.Fakers;

public sealed class CategoryFaker : Faker<Category>
{
    private static readonly List<string> _categories = new()
    {
        "Groceries",
        "Utilities",
        "Insurance",
        "Food delivery",
        "Gym",
        "Shopping",
        "Entertainment",
        "Rent",
    };

    public CategoryFaker()
    {
        RuleFor(x => x.Id, f => f.Random.Guid());
        RuleFor(x => x.Name, f => _categories[f.IndexFaker]);
    }

    public static int CategoryCount => _categories.Count;
}
