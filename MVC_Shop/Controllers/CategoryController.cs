 using Microsoft.AspNetCore.Mvc;
using MVC_Shop.Models;
using MVC_Shop.Repositories.Category;

namespace MVC_Shop.Controllers
{
    public class CategoryController : Controller
    {

        private readonly AppDbContext _context;
        private readonly ICategoryRepository _categoryRepository;
        public CategoryController (AppDbContext context, ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
            _context = context;
        }
        public IActionResult Index()
        {
            IEnumerable<Category> categories = _categoryRepository.Categories;

            return View(categories);
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken] // - для захисту від CSRF-запитів
        public async Task<IActionResult> Create(Category model)
        {
           bool res = await _categoryRepository.IsExistAsync(model.Name);
            if (res)
            {
                return View();
            }
            await _categoryRepository.CreateAsync(model);
            await _categoryRepository.SaveChangesAsync();
            return RedirectToAction("Index");
        }
        public async Task<IActionResult> Update(int id)
        {
            var model = await _categoryRepository.GetByIdAsync(id);
            if (model == null)
            {
                return Index();
            }
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(Category model)
        {
            bool res = await _categoryRepository.IsExistAsync(model.Name);
            if (res)
            {
                return View();
            }
            _categoryRepository.Update(model);
            await _categoryRepository.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Delete(int id)
        {
            await _categoryRepository.DeleteAsync(id);
            await _categoryRepository.SaveChangesAsync();
            return RedirectToAction("Index");

        }

    }
}
