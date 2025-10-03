using Microsoft.EntityFrameworkCore;

namespace MVC_Shop.Repositories.Category
{
    public interface ICategoryRepository
    {
        IQueryable<Models.Category> Categories { get; }
        Task DeleteAsync(int id);
        Task SaveChangesAsync();
        Task CreateAsync(Models.Category category);
        void Update(Models.Category category);
        Task <Models.Category?> GetByIdAsync(int id);
        Task<Models.Category?> GetByNameAsync(string name);
        Task <bool> IsExistAsync(string name);
    }
}
