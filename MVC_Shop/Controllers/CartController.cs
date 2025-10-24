using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MVC_Shop;
using MVC_Shop.Services;
using MVC_Shop.ViewModels;
using MVC_Shop.ViewModels.Product;



namespace PD421_MVC_Shop.Controllers
{
    public class CartController : Controller
    {
        private readonly AppDbContext _context;

        public CartController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            // Session items
            var cartItems = HttpContext.Session.GetCartItems();
            var itemsId = cartItems.Select(i => i.ProductId);

            // Products and create cartVM
            var products = _context.Products
                .Where(p => itemsId.Contains(p.Id))
                .ToList();

            var productsCart = products.Select(p => new ProductCartVM
            {
                Product = p,
                Count = cartItems.FirstOrDefault(i => i.ProductId == p.Id)!.Count
            });

            return View(productsCart);
        }

        public IActionResult IncreaseCount(int productId)
        {
            var amount = _context.Products
                .FirstOrDefault(p => p.Id == productId)?.Count;

            HttpContext.Session.IncreaseCount(productId, amount ?? 0);
            return RedirectToAction("Index");
        }

        public IActionResult DecreaseCount(int productId)
        {
            HttpContext.Session.DecreaseCount(productId);
            return RedirectToAction("Index");
        }

        public IActionResult Checkout(int productId)
        {
            var cartItems = HttpContext.Session.GetCartItems();
            var itemsId = cartItems.Select(i => i.ProductId);

            var products = _context.Products
                .Where(p => itemsId.Contains(p.Id))
                .AsNoTracking()
                .ToList();

            foreach (var product in products)
            {
                int count = cartItems.Find(i => i.ProductId == product.Id)?.Count ?? 0;
                if (count > 0)
                {
                    product.Count -= count;
                    _context.Products.Update(product);
                }
            }
            _context.SaveChanges();
            HttpContext.Session.ClearCart();

            return RedirectToAction("Index", "Home");
        }
    }
}