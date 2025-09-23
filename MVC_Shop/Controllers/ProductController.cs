using Microsoft.AspNetCore.Mvc;

namespace MVC_Shop.Controllers
{
    public class ProductController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
