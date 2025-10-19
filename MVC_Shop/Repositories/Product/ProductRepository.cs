using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using MVC_Shop.Models;
using MVC_Shop.ViewModels;
using System.Linq;

namespace MVC_Shop.Repositories.Product
{
    public class ProductRepository : IProductRepository
    {
        private readonly AppDbContext _context;
        public ProductRepository(AppDbContext context)
        {
            _context = context;
        }

        public IQueryable<Models.Product> Products => _context.Products;

        public async Task CreateAsync(Models.Product product)
        {
            await _context.Products.AddAsync(product);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var model = await _context.Products.FindAsync(id);
            if (model != null)
            {
                _context.Products.Remove(model);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<Models.Product?> GetByIdAsync(int id)
        {
            return await _context.Products.FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<Models.Product?> GetByNameAsync(string name)
        {
            return await _context.Products
                .FirstOrDefaultAsync(c => c.Name.ToLower() == name.ToLower());
        }

        public async Task<List<Models.Product>> GetProductsByCategoryNameAsync(string categoryName)
        {
            return await _context.Products
                .Include(p => p.Category)
                .Where(p => p.Category.Name.ToLower() == categoryName.ToLower())
                .ToListAsync();
        }

        public async Task<List<Models.Product>> GetProductsByPriceRangeAsync(float min, float max)
        {
            return await _context.Products
                .Include(p => p.Category)
                .Where(p => p.Price >= min && p.Price <= max)
                .ToListAsync();
        }

        public async Task<bool> IsExistAsync(string name)
        {
            return await _context.Products.AnyAsync(c => c.Name.ToLower() == name.ToLower());
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

        public void Update(Models.Product product)
        {
            _context.Products.Update(product);
        }
        public async Task<bool> IsExistIdAsync(int id)
        {
            return await _context.Products.AnyAsync(c => c.Id == id);
        }
        public async Task<bool> IsExistAsync(string name, int? excludeId = null)
        {
            if (excludeId.HasValue)
                return await _context.Products.AnyAsync(p => p.Name == name && p.Id != excludeId.Value);
            else
                return await _context.Products.AnyAsync(p => p.Name == name);
        }
        public IQueryable<Models.Product> GetByCategory(string category, Pagination? pagination = null)
        {
            if (string.IsNullOrEmpty(category))
                return GetProducts(pagination);

            category = category.ToLower();

            var products = Products
                .Include(p => p.Category)
                .Where(p => p.Category != null && p.Category.Name.ToLower() == category);

            // Передаємо саме відфільтровану колекцію
            return SetPagination(products, pagination);
        }

        public IQueryable<Models.Product> GetProducts(Pagination? pagination = null)
        {
            var products = Products.Include(p => p.Category);
            return SetPagination(products, pagination);
        }
        private IQueryable<Models.Product> SetPagination(IQueryable<Models.Product> products, Pagination? pagination = null)
        {
            pagination ??= new Pagination();

            pagination.PageCount = (int)Math.Ceiling((double)products.Count() / pagination.PageSize);
            pagination.Page = pagination.Page > pagination.PageCount || pagination.Page <= 0 ? 1 : pagination.Page;

            return products
                .Skip((pagination.Page - 1) * pagination.PageSize)
                .Take(pagination.PageSize);
        }
    }
}
