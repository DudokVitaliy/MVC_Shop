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
        private IWebHostEnvironment _webHostEnvironment;

        public ProductController(AppDbContext context, IProductRepository productRepository, 
            ICategoryRepository categoryRepository, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _productRepository = productRepository;
            _categoryRepository = categoryRepository;
            _webHostEnvironment = webHostEnvironment;
        }
        private string? SaveImage(IFormFile? image)
        {
            if(image == null || image.Length == 0)
            {
                return null;
            }
            string[] types = image.ContentType.Split('/'); 
            if (types[0] != "image" || types.Length != 2)
            {
                return null;
            }
            string imageName = $"{ Guid.NewGuid().ToString()}.{types[1]}";
            string imagePath = Path.Combine(_webHostEnvironment.WebRootPath, "images", imageName);
            
            using (var stream = new FileStream(imagePath, FileMode.Create))
            {
                using (var imageStream = image.OpenReadStream())
                {
                    imageStream.CopyTo(stream);
                }
            }
            return imageName;
        }
        private void DeleteImage(string? imageName)
        {
            if (string.IsNullOrEmpty(imageName))
                return;

            string fullPath = Path.Combine(_webHostEnvironment.WebRootPath, "images", imageName);
            if (System.IO.File.Exists(fullPath))
            {
                System.IO.File.Delete(fullPath);
            }
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
        public async Task<IActionResult> Create([FromForm] CreateProductVM viewModel)
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
                Image = SaveImage(viewModel.Image),
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
                ViewBag.Categories = new SelectList(_categoryRepository.Categories, "Id", "Name", viewModel.CategoryId);
                return View(viewModel);
            }

            if (await _productRepository.IsExistAsync(viewModel.Name, viewModel.Id))
            {
                ModelState.AddModelError("UniqueNameError", $"Товар з іменем '{viewModel.Name}' вже існує!");
                ViewBag.Categories = new SelectList(_categoryRepository.Categories, "Id", "Name", viewModel.CategoryId);
                return View(viewModel);
            }

            var model = await _productRepository.GetByIdAsync(viewModel.Id);
            if (model == null)
            {
                return RedirectToAction("Index");
            }

            if (viewModel.NewImage != null && viewModel.NewImage.Length > 0)
            {
                DeleteImage(model.Image);
                model.Image = SaveImage(viewModel.NewImage);
            }

            model.Name = viewModel.Name;
            model.Description = viewModel.Description;
            model.Price = viewModel.Price ?? 0;
            model.Count = viewModel.Count ?? 0;
            model.CategoryId = viewModel.CategoryId;

            _productRepository.Update(model);
            await _productRepository.SaveChangesAsync();

            return RedirectToAction("Index");
        }


        public async Task<IActionResult> Delete(int id)
        {
            var product = await _productRepository.GetByIdAsync(id);
            if (product != null)
            {
                DeleteImage(product.Image);
                await _productRepository.DeleteAsync(id);
            }
            return RedirectToAction("Index");
        }
    }
}
