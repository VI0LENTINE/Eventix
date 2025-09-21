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
            List<Event> events = new List<Event>();

            Event event1 = new Event();
            event1.EventId = 1;
            event1.Name = "Concert";
            event1.Description = "Music Concert";
            event1.EventDate = DateTime.Now;

            Event event2 = new Event();
            event2.EventId = 2;
            event2.Name = "Comedy";
            event2.Description = "Comedy Show";
            event2.EventDate = DateTime.Now;

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
