using BankDataAPI.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");


DataSeeding.EmptyDatabase();
DatabaseIntegration.CreateTable();
DataSeeding.SeedTestUsers();
DataSeeding.seedAccounts();
DataSeeding.seedTransactions();
DataSeeding.seedAdmin();



app.Run();
