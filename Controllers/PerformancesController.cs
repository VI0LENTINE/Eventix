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
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;


namespace Eventix.Controllers
{
    [Authorize]
    public class PerformancesController : Controller
    {
        private readonly EventixContext _context;
        private readonly BlobServiceClient _blobService;

        public PerformancesController(EventixContext context, BlobServiceClient blobService)
        {
            _context = context;
            _blobService = blobService;
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
                .Include(p => p.Purchases)
                .Include(p => p.Category)
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

                    var container = _blobService.GetBlobContainerClient("performance-images");
                    await container.CreateIfNotExistsAsync();
                    await container.SetAccessPolicyAsync(PublicAccessType.Blob);

                    var blob = container.GetBlobClient(filename);
                    await blob.UploadAsync(performance.FormFile.OpenReadStream(), overwrite: true);

                    performance.ImagePath = blob.Uri.ToString();
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

            if (!ModelState.IsValid)
                return View(performance);

            try
            {
                //
                // Get the existing DB record
                //
                var existingPerformance = await _context.Performance
                    .AsNoTracking()
                    .FirstOrDefaultAsync(p => p.PerformanceId == id);

                if (existingPerformance == null)
                    return NotFound();

                //
                // Blob container reference
                //
                var container = _blobService.GetBlobContainerClient("performance-images");
                await container.CreateIfNotExistsAsync();
                await container.SetAccessPolicyAsync(PublicAccessType.Blob);

                //
                // If new file uploaded, upload new blob, delete old blob
                //
                if (performance.FormFile != null)
                {
                    string newFileName = Guid.NewGuid().ToString() + Path.GetExtension(performance.FormFile.FileName);

                    // Upload new blob
                    var newBlob = container.GetBlobClient(newFileName);
                    await newBlob.UploadAsync(performance.FormFile.OpenReadStream(), overwrite: true);

                    // Delete old blob
                    if (!string.IsNullOrEmpty(existingPerformance.ImagePath))
                    {
                        // Extract filename if ImagePath is URL
                        string oldFilename = existingPerformance.ImagePath.Split('/').Last();
                        var oldBlob = container.GetBlobClient(oldFilename);
                        await oldBlob.DeleteIfExistsAsync();
                    }

                    // Save new blob URL or filename
                    performance.ImagePath = newBlob.Uri.ToString();
                }
                else
                {
                    // KEEP the old filename / URL
                    performance.ImagePath = existingPerformance.ImagePath;
                }

                //
                // Update record in DB
                //
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
    }
}
