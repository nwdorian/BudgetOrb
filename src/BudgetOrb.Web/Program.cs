using BudgetOrb.Application;
using BudgetOrb.Infrastructure;
using BudgetOrb.Web.Extensions;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddApplication();

builder.Services.AddControllersWithViews();

WebApplication app = builder.Build();

app.UseExceptionHandler("/Errors/Error");

if (!app.Environment.IsDevelopment())
{
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(name: "default", pattern: "{controller=Transactions}/{action=Index}/{id?}").WithStaticAssets();

await app.ApplyMigrations();

if (!app.Environment.IsProduction())
{
    await app.SeedDatabase();
}

await app.RunAsync();
