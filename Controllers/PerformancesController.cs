using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Eventix.Data;
using Eventix.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Eventix.Controllers
{
    [Authorize]
    public class PerformancesController : Controller
    {
        private readonly EventixContext _context;
        private readonly IConfiguration _configuration;
        private readonly BlobContainerClient _containerClient;

        public PerformancesController(EventixContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;

            var connectionString = _configuration.GetConnectionString("AzureStorage");
            var containerName = "eventix-uploads";
            _containerClient = new BlobContainerClient(connectionString, containerName);
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
            // performance.CreateDate = DateTime.Now;

            // Validate user input
            if (ModelState.IsValid)
            {
                //
                // Step 1: Save the file (optionally)
                //
                if (performance.FormFile != null)
                {
                    // Upload file to Azure Blob Storage
                    IFormFile fileUpload = performance.FormFile;

                    // Create a unique filename for the blob
                    string blobName = Guid.NewGuid().ToString() + "_" + fileUpload.FileName;
                    var blobClient = _containerClient.GetBlobClient(blobName);

                    using (var stream = fileUpload.OpenReadStream())
                    {
                        await blobClient.UploadAsync(stream, new BlobHttpHeaders
                        {
                            ContentType = fileUpload.ContentType
                        });
                    }

                    // Assign the blob URL to the record to save in the database
                    performance.ImagePath = blobClient.Uri.ToString();
                }
                else
                {
                    // Placeholder image if no image chosen
                    performance.ImagePath = "https://nscc0516070storageblob.blob.core.windows.net/eventix-uploads/microphone.jpg";
                }

                //
                // Step 2: Save record in database
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
                        var blobContainerClient = new BlobContainerClient(_configuration.GetConnectionString("AzureStorage"), "eventix-uploads");
                        var blobClient = blobContainerClient.GetBlobClient(newFileName);

                        using (var stream = performance.FormFile.OpenReadStream())
                        {
                            await blobClient.UploadAsync(stream, true);
                        }

                        // delete the old file
                        if (!string.IsNullOrEmpty(existingPerformance?.ImagePath))
                        {
                            try
                            {
                                var oldBlobUri = new Uri(existingPerformance.ImagePath);
                                string oldBlobName = Path.GetFileName(oldBlobUri.LocalPath);
                                var oldBlobClient = blobContainerClient.GetBlobClient(oldBlobName);
                                await oldBlobClient.DeleteIfExistsAsync();
                            }
                            catch (Exception ex)
                            {
                                // optional: log the error instead of breaking the edit flow
                                Console.WriteLine($"Error deleting old blob: {ex.Message}");
                            }
                        }

                        // set the new filename in the db record
                        performance.ImagePath = blobClient.Uri.ToString();
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

            // If model state invalid, redisplay form
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
            if (performance == null)
                return NotFound();

            // Safely delete image from Azure Blob Storage (if valid)
            if (!string.IsNullOrEmpty(performance.ImagePath) &&
                Uri.IsWellFormedUriString(performance.ImagePath, UriKind.Absolute))
            {
                try
                {
                    var blobUri = new Uri(performance.ImagePath);
                    string blobName = Path.GetFileName(blobUri.LocalPath);

                    var blobContainerClient = new BlobContainerClient(
                        _configuration.GetConnectionString("AzureStorage"),
                        "eventix-uploads"
                    );

                    var blobClient = blobContainerClient.GetBlobClient(blobName);
                    await blobClient.DeleteIfExistsAsync();
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error deleting blob: {ex.Message}");
                }
            }

            // Remove the performance record from the database
            _context.Performance.Remove(performance);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index), "Home");
        }


    }
}
