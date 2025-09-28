using Microsoft.AspNetCore.Mvc;
using MVC_Shop.Models;

namespace MVC_Shop.Controllers
{
    public class CategoryController : Controller
    {

        private readonly AppDbContext _context;
        public CategoryController (AppDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            IEnumerable<Category> categories = _context.Categories;

            return View(categories);
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken] // - для захисту від CSRF-запитів
        public IActionResult Create(Category model)
        {
           bool res = _context.Categories.Any(c => c.Name.ToLower() == model.Name.ToLower());
            if (res)
            {
                return View();
            }
            _context.Categories.Add(model);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }
        public IActionResult Update(int id)
        {
            var model = _context.Categories.Find(id);
            if (model == null)
            {
                return Index();
            }
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Update(Category model)
        {
            bool res = _context.Categories.Any(c => c.Name.ToLower() == model.Name.ToLower());
            if (res)
            {
                return View();
            }
            _context.Update(model);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }

        public IActionResult Delete(int id)
        {
            var model = _context.Categories.Find(id);
            if (model == null)
            {
                return Index();
            }
            _context.Categories.Remove(model);
            _context.SaveChanges();
            return RedirectToAction("Index");

        }

    }
}
