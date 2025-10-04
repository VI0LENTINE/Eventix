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
            event1.ImagePath = "/images/concert.jpg";
            event1.PerformanceId = 1;
            event1.Name = "Concert";
            event1.Description = "Live Music Concert";
            event1.Location = "1800 Argyle St, Halifax, NS B3J 2V9";
            event1.Host = "The Does";
            event1.PerformanceDate = new DateTime(2025, 10, 31, 21, 45, 00);
            event1.EndDate = new DateTime(2025, 11, 01, 00, 45, 00);
            event1.CategoryId = 1;

            Performance event2 = new Performance();
            event2.ImagePath = "/images/comedy.jpg";
            event2.PerformanceId = 2;
            event2.Name = "Comedy";
            event2.Description = "Live Comedy Show";
            event2.Location = "1575 Argyle St Downstairs, Halifax, NS B3J 2B2";
            event2.Host = "John Doe";
            event2.PerformanceDate = new DateTime(2025, 10, 16, 19, 30, 00);
            event2.EndDate = new DateTime(2025, 10, 16, 21, 30, 00);
            event2.CategoryId = 2;

            Performance event3 = new Performance();
            event3.ImagePath = "images/basketball.jpg";
            event3.PerformanceId = 3;
            event3.Name = "Basketball";
            event3.Description = "Live Sports Game";
            event3.Location = "1800 Argyle St, Halifax, NS B3J 2V9";
            event3.Host = "Jane Doe";
            event3.PerformanceDate = new DateTime(2025, 10, 20, 17, 40, 00);
            event3.EndDate = new DateTime(2025, 10, 20, 19, 40, 00);
            event3.CategoryId = 3;

            events.Add(event1);
            events.Add(event2);
            events.Add(event3);

            //return View(events);

            var sortedEvents = events
                .OrderBy(e => e.PerformanceDate)
                .ToList();

            return View(sortedEvents);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
