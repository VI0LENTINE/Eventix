using Eventix.Data;
using Eventix.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace Eventix.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly EventixContext _context;

        public HomeController(ILogger<HomeController> logger, EventixContext context)
        {
            _logger = logger;
            _context = context;
        }

        // GET: Performances
        public async Task<IActionResult> Index()
        {
            var performances = await _context.Performance
                .OrderByDescending(m => m.PerformanceDate)
                .Include(p => p.Category)
                .Include(p => p.Purchases)
                .ToListAsync();

            return View(performances);
        }

        // GET: Performances/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var photo = await _context.Performance
                .Include(p => p.Category)
                .FirstOrDefaultAsync(m => m.PerformanceId == id);

            if (photo == null)
            {
                return NotFound();
            }

            return View(photo);
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
