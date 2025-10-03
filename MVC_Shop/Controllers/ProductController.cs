using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MVC_Shop.Models;
using MVC_Shop.Repositories.Product;

namespace MVC_Shop.Controllers
{
    public class ProductController : Controller
    {
        private readonly IProductRepository _productRepository;
        private readonly AppDbContext _context;

        public ProductController(AppDbContext context, IProductRepository productRepository)
        {
            _context = context;
            _productRepository = productRepository;
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
            ViewBag.Categories = new SelectList(_context.Categories, "Id", "Name");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Product model)
        {
            if (await _productRepository.IsExistAsync(model.Name) && 
                await _productRepository.IsExistIdAsync(model.Id))
            {
                ViewBag.Categories = new SelectList(_context.Categories, "Id", "Name");
                return View(model);
                
            }

            await _productRepository.CreateAsync(model);
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Update(int id)
        {
            var model = await _productRepository.GetByIdAsync(id);
            if (model == null)
            {
                return RedirectToAction("Index");
            }
            ViewBag.Categories = new SelectList(_context.Categories, "Id", "Name", model.CategoryId);
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(Product model)
        {
            if (await _productRepository.IsExistAsync(model.Name) &&
                !_context.Products.Any(c => c.Id == model.Id))
            {
                ViewBag.Categories = new SelectList(_context.Categories, "Id", "Name", model.CategoryId);
                return View(model);
            }

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
