using MVC_Shop.Models;
using MVC_Shop.ViewModels;
namespace MVC_Shop.Repositories.Product
{
    public interface IProductRepository
    {
        IQueryable<Models.Product> Products { get; }
        Task CreateAsync(Models.Product product);
        Task DeleteAsync(int id);
        void Update(Models.Product product);
        Task SaveChangesAsync();
        Task<Models.Product?> GetByIdAsync(int id);
        Task<Models.Product?> GetByNameAsync(string name);
        Task<List<Models.Product>> GetProductsByCategoryNameAsync(string categoryName);
        Task<List<Models.Product>> GetProductsByPriceRangeAsync(float min, float max);
        Task<bool> IsExistAsync(string name);
        Task<bool> IsExistIdAsync(int id);
        Task<bool> IsExistAsync(string name, int? excludeId = null);
        public IQueryable<Models.Product> GetByCategory( string category, Pagination? pagination = null);
        public IQueryable<Models.Product> GetProducts(Pagination? pagination = null);

    }
}
