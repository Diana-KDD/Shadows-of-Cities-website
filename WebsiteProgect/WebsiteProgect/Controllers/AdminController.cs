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
    }
}
