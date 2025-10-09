 using Microsoft.AspNetCore.Mvc;
using MVC_Shop.Models;
using MVC_Shop.Repositories.Category;
using MVC_Shop.ViewModels.Category;

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
        public async Task<IActionResult> Create(CreateCategoryVM viewModel)
        {
            if (!ModelState.IsValid)
            {
                return View(viewModel);
            }
            bool res = await _categoryRepository.IsExistAsync(viewModel.Name ?? string.Empty);
            if (res)
            {
                ModelState.AddModelError("UniqueError", $"Категорія '{viewModel.Name}' вже існує!");
                return View(viewModel);
            }
            var model = new Category { Name = viewModel.Name ?? string.Empty};
            await _categoryRepository.CreateAsync(model);
            await _categoryRepository.SaveChangesAsync();
            return RedirectToAction("Index");
        }
        public async Task<IActionResult> Update(int id)
        {
            var model = await _categoryRepository.GetByIdAsync(id);
            if (model == null)
            {
                return RedirectToAction("Index");
            }

            var viewModel = new UpdateCategoryVM
            {
                Id = model.Id,
                Name = model.Name
            };

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(UpdateCategoryVM viewModel)
        {
            if (!ModelState.IsValid)
            {
                return View(viewModel);
            }

            bool res = await _categoryRepository.IsExistAsync(viewModel.Name ?? string.Empty);
            if (res)
            {
                ModelState.AddModelError("UniqueNameError", $"Категорія '{viewModel.Name}' вже існує");
                return View(viewModel);
            }

            var model = await _categoryRepository.GetByIdAsync(viewModel.Id);
            if (model != null)
            {
                model.Name = viewModel.Name ?? "";
                _categoryRepository.Update(model);
                await _categoryRepository.SaveChangesAsync();
            }
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
