using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebsiteProgect.Data;
using WebsiteProgect.Models;

namespace WebsiteProgect.Controllers
{
    public class AdminController:Controller
    {
        private readonly AppDbContext _context;
        private readonly ILogger<AdminController> _logger;

        public AdminController(AppDbContext context, ILogger<AdminController> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<IActionResult> Index()
        {
            var places = await _context.Places
                    .Include(p => p.Images)
                    .Include(p => p.City)
                        .ThenInclude(c => c.Country)
                    .ToListAsync();

            return View(places);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var place = await _context.Places
                .Include(p=>p.City)
                    .ThenInclude(c=>c.Country)
                .Include(p => p.Category)
                .FirstOrDefaultAsync(p=>p.Id == id);
            
            if(place == null)
                return NotFound();

            return View(place);
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var place = await _context.Places
                .Include(p=>p.Images)
                .FirstOrDefaultAsync(p=>p.Id==id);

            if(place == null)
                return NotFound();

            foreach(var image in place.Images)
            {
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", image.ImagePath.TrimStart('/'));
                if(System.IO.File.Exists(filePath))
                {
                    System.IO.File.Delete(filePath);
                }
            }
            _context.Places.Remove(place);
            await _context.SaveChangesAsync();

            _logger.LogInformation($"Удалена карточка с местом: {place.Name}");
            return RedirectToAction("Index");
        }
    }
}
