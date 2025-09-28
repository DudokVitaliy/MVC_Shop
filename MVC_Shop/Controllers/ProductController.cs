using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MVC_Shop.Models;

namespace MVC_Shop.Controllers
{
    public class ProductController : Controller
    {
        private readonly AppDbContext _context;
        public ProductController(AppDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            IEnumerable<Product> products = _context.Products
                .Include(p => p.Category);
            return View(products);
        }
        public IActionResult Create()
        {
            ViewBag.Categories = new SelectList(_context.Categories, "Id", "Name");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Product model)
        {
            bool res = _context.Products.Any(c => c.Name.ToLower() == model.Name.ToLower());
            if (res)
            {
                return View();
            }
            _context.Products.Add(model);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }
        public IActionResult Update(int id)
        {
            var model = _context.Products.Find(id);
            if (model == null)
            {
                return Index();
            }
            ViewBag.Categories = new SelectList(_context.Categories, "Id", "Name");
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Update(Product model)
        {
            bool res = _context.Products.Any(c => c.Name.ToLower() == model.Name.ToLower() && c.Id != model.Id);
            if (res)
            {
                ViewBag.Categories = new SelectList(_context.Categories, "Id", "Name");
                return View(model);
            }

            _context.Update(model);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }

        public IActionResult Delete(int id)
        {
            var model = _context.Products.Find(id);
            if (model == null)
            {
                return Index();
            }
            _context.Products.Remove(model);
            _context.SaveChanges();
            return RedirectToAction("Index");

        }
    }
}
