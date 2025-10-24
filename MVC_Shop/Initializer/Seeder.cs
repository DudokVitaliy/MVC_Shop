using Microsoft.AspNetCore.Identity;
using MVC_Shop;
using MVC_Shop.Models;

namespace PD421_MVC_Shop.Initializer
{
    public static class Seeder
    {
        public static async Task Seed(this IApplicationBuilder app)
        {
            try
            {
                using var scope = app.ApplicationServices.CreateScope();
                var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
                var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

                await context.Database.EnsureCreatedAsync();

                string[] roles = { "admin", "user" };

                foreach (var role in roles)
                {
                    if (!await roleManager.RoleExistsAsync(role))
                    {
                        await roleManager.CreateAsync(new IdentityRole(role));
                        Console.WriteLine($"✅ Роль '{role}' створено.");
                    }
                }

                if (await userManager.FindByEmailAsync("admin@mail.com") == null)
                {
                    var admin = new ApplicationUser
                    {
                        UserName = "admin",
                        Email = "admin@mail.com",
                        EmailConfirmed = true
                    };

                    var result = await userManager.CreateAsync(admin, "Qwerty123!");
                    if (result.Succeeded)
                    {
                        await userManager.AddToRoleAsync(admin, "admin");
                        Console.WriteLine("Користувача 'admin' створено.");
                    }
                    else
                    {
                        Console.WriteLine("Помилка створення admin: " + string.Join(", ", result.Errors.Select(e => e.Description)));
                    }
                }

                if (await userManager.FindByEmailAsync("user@mail.com") == null)
                {
                    var user = new ApplicationUser
                    {
                        UserName = "user",
                        Email = "user@mail.com",
                        EmailConfirmed = true
                    };

                    var result = await userManager.CreateAsync(user, "Qwerty123!");
                    if (result.Succeeded)
                    {
                        await userManager.AddToRoleAsync(user, "user");
                    }
                    else
                    {
                        Console.WriteLine("Помилка створення user: " + string.Join(", ", result.Errors.Select(e => e.Description)));
                    }
                }

                if (!context.Categories.Any())
                {
                    var categories = new Category[]
                    {
                        new Category { Name = "Відеокарти" },
                        new Category { Name = "Процесори" },
                        new Category { Name = "Ноутбуки" },
                        new Category { Name = "Монітори" },
                        new Category { Name = "SSD диски" }
                    };

                    await context.Categories.AddRangeAsync(categories);
                    await context.SaveChangesAsync();

                    var products = new Product[]
                    {
                        new Product { Name = "Intel Core i5-12400F", Description = "6-ядерний процесор", Price = 7499, Count = 18, Category = categories[1] },
                        new Product { Name = "Asus TUF Gaming F15", Description = "Ігровий ноутбук", Price = 28999, Count = 9, Category = categories[2] },
                        new Product { Name = "Samsung 980 Pro 1TB NVMe", Description = "Швидкий SSD", Price = 5499, Count = 20, Category = categories[4] }
                    };

                    await context.Products.AddRangeAsync(products);
                    await context.SaveChangesAsync();
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine("Seeder error: " + ex.Message);
            }
        }
    }
}
