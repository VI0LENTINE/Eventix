using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Eventix.Data;
using Eventix.Models;
using Microsoft.AspNetCore.Authorization;

namespace Eventix.Controllers
{
    [Authorize]
    public class PerformancesController : Controller
    {
        private readonly EventixContext _context;

        public PerformancesController(EventixContext context)
        {
            _context = context;
        }

        // GET: Performances
        public async Task<IActionResult> Index()
        {
            return View(await _context.Performance.ToListAsync());
        }

        // GET: Performances/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var performance = await _context.Performance
                .FirstOrDefaultAsync(m => m.PerformanceId == id);
            if (performance == null)
            {
                return NotFound();
            }

            return View(performance);
        }

        // GET: Performances/Create
        public IActionResult Create()
        {
            //ViewData["CategoryId"] = new SelectList(_context.Category, "CategoryId", "Name");
            return View();
        }

        // POST: Performances/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("PerformanceId,Name,Description,PerformanceDate,EndDate,ImagePath,FormFile,Host,Location,CategoryId")] Performance performance)
        {
            // Initialize values
            //performance.CreateDate = DateTime.Now;

            // Validate user input
            if (ModelState.IsValid)
            {
                //
                // Step 1: save the file (optionally)
                //
                if (performance.FormFile != null)
                {
                    // Create a unique filename using a Guid          
                    string filename = Guid.NewGuid().ToString() + Path.GetExtension(performance.FormFile.FileName); // f81d4fae-7dec-11d0-a765-00a0c91e6bf6.jpg

                    // Initialize the filename in photo record
                    performance.ImagePath = filename;

                    // Get the file path to save the file. Use Path.Combine to handle diffferent OS
                    string saveFilePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", filename);

                    // Save file
                    using (FileStream fileStream = new FileStream(saveFilePath, FileMode.Create))
                    {
                        await performance.FormFile.CopyToAsync(fileStream);
                    }
                }

                else
                {
                    // Placeholder image if no image chosen
                    performance.ImagePath = "microphone.jpg"; 
                }

                //
                // Step 2: save record in database
                //

                _context.Add(performance);

                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index), "Home"); // Home index page
            }

            ViewData["CategoryId"] = new SelectList(_context.Set<Category>(), "CategoryId", "Title", performance.CategoryId);

            return View(performance);
        }


        // GET: Performances/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var performance = await _context.Performance.FindAsync(id);
            if (performance == null)
            {
                return NotFound();
            }
            return View(performance);
        }

        // POST: Performances/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("PerformanceId,Name,Description,PerformanceDate,EndDate,ImagePath,FormFile,Host,Location,CategoryId")] Performance performance)
        {
            if (id != performance.PerformanceId)
                return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    // Get existing record to preserve old file if needed
                    var existingPerformance = await _context.Performance.AsNoTracking()
                        .FirstOrDefaultAsync(p => p.PerformanceId == id);

                    // step 1: save the file
                    if (performance.FormFile != null)
                    {
                        // determine new filename
                        string newFileName = Guid.NewGuid().ToString() + Path.GetExtension(performance.FormFile.FileName);

                        // upload the new file
                        string savePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", newFileName);
                        using (FileStream stream = new FileStream(savePath, FileMode.Create))
                        {
                            await performance.FormFile.CopyToAsync(stream);
                        }

                        // delete the old file
                        if (!string.IsNullOrEmpty(existingPerformance?.ImagePath))
                        {
                            string oldFilePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", existingPerformance.ImagePath);
                            if (System.IO.File.Exists(oldFilePath))
                                System.IO.File.Delete(oldFilePath);
                        }

                        // set the new filename in the db record
                        performance.ImagePath = newFileName;
                    }
                    else
                    {
                        // No new file uploaded, keep existing filename
                        performance.ImagePath = existingPerformance?.ImagePath;
                    }

                    // step 2: save in database
                    _context.Update(performance);
                    await _context.SaveChangesAsync();

                    return RedirectToAction(nameof(Index), "Home");
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.Performance.Any(e => e.PerformanceId == performance.PerformanceId))
                        return NotFound();
                    else
                        throw;
                }
            }

            return View(performance);
        }

        // GET: Performances/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var performance = await _context.Performance
                .FirstOrDefaultAsync(m => m.PerformanceId == id);
            if (performance == null)
            {
                return NotFound();
            }

            return View(performance);
        }

        // POST: Performances/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var performance = await _context.Performance.FindAsync(id);
            if (performance != null)
            {
                _context.Performance.Remove(performance);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index), "Home");
        }

        //private bool PerformanceExists(int id)
        //{
        //    return _context.Performance.Any(e => e.PerformanceId == id);
        //}
    }
}
