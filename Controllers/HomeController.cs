using System.Diagnostics;
using Eventix.Models;
using Microsoft.AspNetCore.Mvc;

namespace Eventix.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            List<Performance> events = new List<Performance>();

            Performance event1 = new Performance();
            event1.PerformanceId = 1;
            event1.Name = "Concert";
            event1.Description = "Music Concert";
            event1.PerformanceDate = DateTime.Now;

            Performance event2 = new Performance();
            event2.PerformanceId = 2;
            event2.Name = "Comedy";
            event2.Description = "Comedy Show";
            event2.PerformanceDate = DateTime.Now;

            events.Add(event1);
            events.Add(event2);

            return View(events);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
