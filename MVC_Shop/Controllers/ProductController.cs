using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MVC_Shop.Models;
using MVC_Shop.Repositories.Category;
using MVC_Shop.Repositories.Product;
using MVC_Shop.ViewModels.Product;

namespace MVC_Shop.Controllers
{
    public class ProductController : Controller
    {
        private readonly IProductRepository _productRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly AppDbContext _context;

        public ProductController(AppDbContext context, IProductRepository productRepository, ICategoryRepository categoryRepository)
        {
            _context = context;
            _productRepository = productRepository;
            _categoryRepository = categoryRepository;
        }

        public IActionResult Index()
        {
            IEnumerable<Product> products = _productRepository.Products
                .Include(p => p.Category);
            return View(products);
        }

        public async Task<IActionResult> ByCategory(string categoryName)
        {
            var products = await _productRepository.GetProductsByCategoryNameAsync(categoryName);
            return View("Index", products);
        }

        public async Task<IActionResult> ByPrice(float min, float max)
        {
            var products = await _productRepository.GetProductsByPriceRangeAsync(min, max);
            return View("Index", products);
        }

        public IActionResult Create()
        {
            ViewBag.Categories = new SelectList(_categoryRepository.Categories, "Id", "Name");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateProductVM viewModel)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Categories = new SelectList(_categoryRepository.Categories, "Id", "Name");
                return View(viewModel);
            }
            if (await _productRepository.IsExistAsync(viewModel.Name))
            {
                ModelState.AddModelError("UniqueNameError", $"Товар з іменем '{viewModel.Name}' вже існує!");
                ViewBag.Categories = new SelectList(_categoryRepository.Categories, "Id", "Name");
                return View(viewModel);

            }
            var product = new Product
            {
                Name = viewModel.Name,
                Description = viewModel.Description,
                Price = viewModel.Price ?? 0,
                Image = viewModel.Image,
                Count = viewModel.Count ?? 0,
                CategoryId = viewModel.CategoryId
            };
            await _productRepository.CreateAsync(product);
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Update(int id)
        {
            var model = await _productRepository.GetByIdAsync(id);
            if (model == null)
            {
                return RedirectToAction("Index");
            }
            var viewModel = new UpdateProductVM
            {
                Id = model.Id,
                Name = model.Name,
                Description = model.Description,
                Price = model.Price,
                Image = model.Image,
                Count = model.Count,
                CategoryId = model.CategoryId
            };
            ViewBag.Categories = new SelectList(_categoryRepository.Categories, "Id", "Name", model.CategoryId);
            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(UpdateProductVM viewModel)
        {
            if (!ModelState.IsValid)
            {
                return View(viewModel);
            }
            if (await _productRepository.IsExistAsync(viewModel.Name, viewModel.Id))
            {
                ModelState.AddModelError("UniqueNameError", $"Товар з іменем '{viewModel.Name}' вже існує!");
                ViewBag.Categories = new SelectList(_categoryRepository.Categories, "Id", "Name");
                return View(viewModel);

            }
            if (await _productRepository.IsExistAsync(viewModel.Name) &&
                !_productRepository.Products.Any(c => c.Id == viewModel.Id))
            {
                ViewBag.Categories = new SelectList(_categoryRepository.Categories, "Id", "Name", viewModel.CategoryId);
                return View(viewModel);
            }
            var model = await _productRepository.GetByIdAsync(viewModel.Id);
            if (model == null)
            {
                return RedirectToAction("Index");
            }
            model.Name = viewModel.Name;
            model.Description = viewModel.Description;
            model.Price = viewModel.Price ?? 0;
            model.Image = viewModel.Image;
            model.Count = viewModel.Count ?? 0;
            model.CategoryId = viewModel.CategoryId;

            _productRepository.Update(model);
            await _productRepository.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Delete(int id)
        {
            await _productRepository.DeleteAsync(id);
            return RedirectToAction("Index");
        }
    }
}
