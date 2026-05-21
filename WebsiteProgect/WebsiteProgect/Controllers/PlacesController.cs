using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebsiteProgect.Data;

namespace WebsiteProgect.Controllers
{
    public class PlacesController: Controller
    {
        private readonly AppDbContext _context;
        private readonly ILogger<PlacesController> _logger;

        public PlacesController(AppDbContext context, ILogger<PlacesController> logger)
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
                    .Include(p=>p.Category)
                    .ToListAsync();

            return View(places);
        }

        public async Task<IActionResult> Details(int id)
        {
            var place = await _context.Places
                .Where(p => p.Id == id)
                .Include(p => p.Images)
                .Include(p => p.City)
                    .ThenInclude(c => c.Country)
                .Include(p => p.Category)
                .FirstOrDefaultAsync();

            if (place == null)
            {
                return NotFound();
            }

            _logger.LogInformation($"Details: {id}");
            return View(place);
        }


    }
}
