using Microsoft.AspNetCore.Mvc;

namespace MVC_Shop.Controllers
{
    public class PlanetController : Controller
    {
        public IActionResult BestPlaces()
        {
            return View();
        }
    }
}
