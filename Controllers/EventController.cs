using Microsoft.AspNetCore.Mvc;

namespace Eventix.Controllers
{
    public class EventController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Events()
        {
            return View();
        }
    }
}
