
using Microsoft.AspNetCore.Identity;
using MVC_Shop;
using MVC_Shop.Models;

namespace PD421_MVC_Shop.Initializer
{
    public static class Seeder
    {
        public static async void Seed(this IApplicationBuilder app)
        {
            try
            {
                using var scope = app.ApplicationServices.CreateScope();
                var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();

                // Categories and Products
                if (!context.Categories.Any())
                {
                    var categories = new Category[5]
                    {
                        new Category { Name = "Відеокарти" },
                        new Category { Name = "Процесори" },
                        new Category { Name = "Ноутбуки" },
                        new Category { Name = "Монітори" },
                        new Category { Name = "SSD диски" }
                    };

                    await context.Categories.AddRangeAsync(categories);
                    await context.SaveChangesAsync();

                    var products = new Product[25]
                    {
                        // Category 1 - Відеокарти
                        new Product { Name = "Asus GeForce RTX 3060 Dual OC 12GB", Description = "Потужна відеокарта для ігор", Price = 13499, Count = 15, Image = null, Category = categories[0] },
                        new Product { Name = "MSI Radeon RX 6600 XT 8GB", Description = "Ігрова відеокарта з оптимальною ціною", Price = 11499, Count = 12, Image = null, Category = categories[0] },
                        new Product { Name = "Gigabyte GeForce RTX 4070 Ti 12GB", Description = "Високопродуктивна відеокарта для 4K", Price = 39999, Count = 5, Image = null, Category = categories[0] },
                        new Product { Name = "Zotac GeForce GTX 1660 Super 6GB", Description = "Бюджетне рішення для Full HD", Price = 7999, Count = 20, Image = null, Category = categories[0] },
                        new Product { Name = "Palit GeForce RTX 3080 GamingPro 10GB", Description = "Преміум сегмент для ентузіастів", Price = 29999, Count = 8, Image = null, Category = categories[0] },

                        // Category 2 - Процесори
                        new Product { Name = "Intel Core i5-12400F", Description = "6-ядерний процесор для геймінгу", Price = 7499, Count = 18, Image = null, Category = categories[1] },
                        new Product { Name = "AMD Ryzen 5 5600X", Description = "Ідеальний баланс ціни та продуктивності", Price = 8199, Count = 14, Image = null, Category = categories[1] },
                        new Product { Name = "Intel Core i9-13900K", Description = "Флагманський процесор для ентузіастів", Price = 25999, Count = 7, Image = null, Category = categories[1] },
                        new Product { Name = "AMD Ryzen 9 7950X", Description = "16 ядер для максимальних навантажень", Price = 28999, Count = 4, Image = null, Category = categories[1] },
                        new Product { Name = "Intel Core i3-12100F", Description = "Бюджетний варіант для офісу", Price = 4699, Count = 22, Image = null, Category = categories[1] },

                        // Category 3 - Ноутбуки
                        new Product { Name = "Asus TUF Gaming F15", Description = "Ігровий ноутбук із RTX 3050 Ti", Price = 28999, Count = 9, Image = null, Category = categories[2] },
                        new Product { Name = "Lenovo IdeaPad 3", Description = "Ноутбук для навчання та роботи", Price = 18999, Count = 11, Image = null, Category = categories[2] },
                        new Product { Name = "HP Pavilion Gaming 17", Description = "17-дюймовий ноутбук для ігор", Price = 32999, Count = 6, Image = null, Category = categories[2] },
                        new Product { Name = "Acer Aspire 5", Description = "Легкий ноутбук для подорожей", Price = 16999, Count = 13, Image = null, Category = categories[2] },
                        new Product { Name = "Apple MacBook Air M2", Description = "Ноутбук з чипом Apple Silicon", Price = 48999, Count = 5, Image = null, Category = categories[2] },

                        // Category 4 - Монітори
                        new Product { Name = "Samsung Odyssey G5 32\"", Description = "Ігровий монітор із вигнутим екраном", Price = 11999, Count = 10, Image = null, Category = categories[3] },
                        new Product { Name = "LG UltraGear 27GN800-B", Description = "27\" IPS 144Hz монітор", Price = 9999, Count = 15, Image = null, Category = categories[3] },
                        new Product { Name = "Dell UltraSharp U2720Q", Description = "4K монітор для роботи з графікою", Price = 16999, Count = 7, Image = null, Category = categories[3] },
                        new Product { Name = "AOC 24G2U", Description = "Бюджетний ігровий 24\" 144Hz", Price = 6799, Count = 18, Image = null, Category = categories[3] },
                        new Product { Name = "Xiaomi Mi Curved 34\"", Description = "Ультраширокий монітор для роботи", Price = 13999, Count = 9, Image = null, Category = categories[3] },

                        // Category 5 - SSD диски
                        new Product { Name = "Samsung 980 Pro 1TB NVMe", Description = "Швидкий SSD для ігор і роботи", Price = 5499, Count = 20, Image = null, Category = categories[4] },
                        new Product { Name = "Kingston A2000 500GB NVMe", Description = "Надійний диск за доступною ціною", Price = 2199, Count = 25, Image = null, Category = categories[4] },
                        new Product { Name = "Crucial MX500 1TB SATA", Description = "Класичний SSD для апгрейду", Price = 3299, Count = 17, Image = null, Category = categories[4] },
                        new Product { Name = "WD Black SN850X 2TB NVMe", Description = "Преміальний ігровий SSD", Price = 8999, Count = 8, Image = null, Category = categories[4] },
                        new Product { Name = "Patriot P210 256GB SATA", Description = "Бюджетний SSD для офісного ПК", Price = 999, Count = 30, Image = null, Category = categories[4] },
                    };

                    await context.Products.AddRangeAsync(products);
                    await context.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
