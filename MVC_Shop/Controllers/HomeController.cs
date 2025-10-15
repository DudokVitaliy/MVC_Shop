using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MVC_Shop.Models;
using MVC_Shop.Repositories.Category;
using MVC_Shop.Repositories.Product;
using System.Diagnostics;

namespace MVC_Shop.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IProductRepository _productRepository;
        private readonly ICategoryRepository _categoryRepository;

        public HomeController(ILogger<HomeController> logger, IProductRepository productRepository, ICategoryRepository categoryRepository)
        {
            _logger = logger;
            _productRepository = productRepository;
            _categoryRepository = categoryRepository;
        }

        public IActionResult Index()
        {
            var viewModel = new ViewModels.Home.HomeVM
            {
                Products = _productRepository.Products.Include(p => p.Category).ToList(),
                Categories = _categoryRepository.Categories.ToList()
            };
            return View(viewModel);
        }

        public IActionResult Privacy()
        {
            return View();
        }
        public IActionResult AboutUs()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        public IActionResult ByCategory(string categoryName)
        {
            var products = _productRepository.Products
                .Where(p => p.Category != null && p.Category.Name == categoryName)
                .Include(p => p.Category)
                .ToList();

            var viewModel = new ViewModels.Home.HomeVM
            {
                Products = products,
                Categories = _categoryRepository.Categories.ToList()
            };

            ViewBag.SelectedCategory = categoryName;
            return View("Index", viewModel);
        }
        public async Task<IActionResult> Detail(int id)
        {
            var product = await _productRepository.Products
                .Include(p => p.Category)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (product == null)
            {
                return RedirectToAction("Index");
            }

            return View(product);
        }
    }
}
