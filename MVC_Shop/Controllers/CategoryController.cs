using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MVC_Shop.Models;
using MVC_Shop.Repositories.Category;
using MVC_Shop.ViewModels.Category;

namespace MVC_Shop.Controllers
{
    [Authorize(Roles = "admin")]
    public class CategoryController : Controller
    {

        private readonly AppDbContext _context;
        private readonly ICategoryRepository _categoryRepository;
        public CategoryController (AppDbContext context, ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
            _context = context;
        }
        public async Task<IActionResult> Index(int page = 1)
        {
            int pageSize = Settings.PaginationPageSize;

            var categoriesQuery = _categoryRepository.Categories.AsQueryable();

            int totalItems = await categoriesQuery.CountAsync();

            var categories = await categoriesQuery
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var model = new CategoryListVM
            {
                Categories = categories,
                CurrentPage = page,
                PageSize = pageSize,
                TotalItems = totalItems
            };

            return View(model);
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
