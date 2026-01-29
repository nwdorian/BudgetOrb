using BudgetOrb.Infrastructure.Database;
using BudgetOrb.Infrastructure.Database.Seeding;
using Microsoft.EntityFrameworkCore;

namespace BudgetOrb.Web.Extensions;

public static class ApplicationBuilderExtensions
{
    public static async Task ApplyMigrations(this IApplicationBuilder app)
    {
        using IServiceScope scope = app.ApplicationServices.CreateScope();
        using ApplicationDbContext context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

        await context.Database.MigrateAsync();
    }

    public static async Task SeedDatabase(this IApplicationBuilder app)
    {
        using IServiceScope scope = app.ApplicationServices.CreateScope();
        SeedData seedData = scope.ServiceProvider.GetRequiredService<SeedData>();

        await seedData.Run();
    }
}
