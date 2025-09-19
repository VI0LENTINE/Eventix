using Microsoft.AspNetCore.Mvc;

namespace Eventix.Controllers
{
    public class TicketsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
