

using Microsoft.EntityFrameworkCore;

namespace MVC_Shop.Repositories.Category
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly AppDbContext _context;
        public CategoryRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task DeleteAsync(int id)
        {
            var model = await _context.Categories.FindAsync(id);
            if (model != null)
            {
                _context.Categories.Remove(model);
                await _context.SaveChangesAsync();
            }
        }
        public async Task SaveChangesAsync()
        {
           await _context.SaveChangesAsync();
        }
        public IQueryable<Models.Category> Categories => _context.Categories;
        public async Task CreateAsync(Models.Category category)
        {
            await _context.Categories.AddAsync(category);
            await _context.SaveChangesAsync();
        }
        public void Update(Models.Category category)
        {
            _context.Categories.Update(category);
        }

        public async Task<Models.Category?> GetByIdAsync(int id)
        {
            return await _context.Categories.FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<Models.Category?> GetByNameAsync(string name)
        {
            return await _context.Categories
                .FirstOrDefaultAsync(c => c.Name.ToLower() == name.ToLower());
        }
        public async Task<bool> IsExistAsync(string name)
        {
            return await _context.Categories.AnyAsync(c => c.Name.ToLower() == name.ToLower());

        }
    }
}
