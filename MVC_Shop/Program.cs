using Microsoft.EntityFrameworkCore;
using MVC_Shop;
using MVC_Shop.Repositories.Category;
using MVC_Shop.Repositories.Product;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// add dataBase context
builder.Services.AddDbContext<AppDbContext>(options =>
{
    //options.UseSqlServer("name = SqlServer");
    string? connectionString = builder.Configuration.GetConnectionString("SqlServer");
    options.UseSqlServer(connectionString);


});
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<IProductRepository, ProductRepository>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();


app.Run();
