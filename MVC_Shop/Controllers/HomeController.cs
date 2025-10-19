using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MVC_Shop.Models;
using MVC_Shop.Repositories.Category;
using MVC_Shop.Repositories.Product;
using MVC_Shop.ViewModels;
using MVC_Shop.ViewModels.Home;
using System.Diagnostics;
using System.Net.WebSockets;

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


        public IActionResult Index(string? category, int? page)
        {
            int currentPage = page ?? 1;

            var pagination = new Pagination
            {
                Page = currentPage,
                PageSize = Settings.PaginationPageSize
            };

            var products =
                !string.IsNullOrEmpty(category)
                ? _productRepository.GetByCategory(category, pagination)
                : _productRepository.GetProducts(pagination);

            var productsList = new ProductListVM
            {
                Pagination = pagination,
                Products = products
            };

            var viewModel = new HomeVM
            {
                Categories = _categoryRepository.Categories,
                CategoryName = category,
                ProductList = new ProductListVM
                {
                    Products = products,
                    Pagination = pagination
                }
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
