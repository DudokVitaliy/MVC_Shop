using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using MVC_Shop;
using MVC_Shop.Models;
using MVC_Shop.Repositories.Category;
using MVC_Shop.Repositories.Product;
using MVC_Shop.Repositories.Users;
using MVC_Shop.Services;
using PD421_MVC_Shop.Initializer;

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


// add identity
builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
{
    options.User.RequireUniqueEmail = true;

    options.Password.RequireDigit = true; // �����
    options.Password.RequiredUniqueChars = 1; // ���������� ������
    options.Password.RequireLowercase = true; // �������� �����
    options.Password.RequireUppercase = true; // ������ �����
    options.Password.RequireNonAlphanumeric = true; // �� ����� � �� �����
    options.Password.RequiredLength = 8; // ��������� �������
})
    .AddEntityFrameworkStores<AppDbContext>()
    .AddDefaultUI()
    .AddDefaultTokenProviders();

builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddTransient<IEmailSender, EmailSender>();
builder.Services.AddScoped<IUsersRepository, UsersRepository>();


// add session
builder.Services.AddHttpContextAccessor();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromHours(1);
    options.Cookie.IsEssential = true;
    options.Cookie.HttpOnly = true;
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseSession();

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();
app.MapRazorPages();
await app.Seed();
app.Run();
