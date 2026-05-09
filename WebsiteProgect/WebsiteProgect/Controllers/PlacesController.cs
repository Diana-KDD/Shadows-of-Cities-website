using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Numerics;
using WebsiteProgect.Data;
using WebsiteProgect.Models;

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

            if (places.Count == 0)
            {
                // Создаём страну
                var country = new Country { Name = "Россия" };
                _context.Countries.Add(country);
                await _context.SaveChangesAsync();

                // Создаём город и привязываем к стране через навигационное свойство
                var city = new City
                {
                    Name = "Тула",
                    CountryId = country.Id,
                    Country = country
                };
                _context.Cities.Add(city);
                await _context.SaveChangesAsync();

                // Создаём категорию
                var category = new Category { Name = "Заводы" };
                _context.Categories.Add(category);
                await _context.SaveChangesAsync();

                // Создаём место с использованием навигационных свойств
                var place = new Place
                {
                    Name = "Сахарорафинадный завод",
                    CategoryId = category.Id,
                    Category = category,
                    CityId = city.Id,
                    City = city,
                    Description = "Завод признан банкротом в 2010 году. По состоянию на 2014 год большая часть оборудования вывезена или распилена. " +
                    "Определенный интерес представляет огромный механизм на ременной тяге на втором этаже, оставшийся видимо еще с царских времен.",
                    History = "Завод был основан в 1873 году, и на то время был третьим крупным предприятием Тулы после Оружейного и Патронного заводов." +
                    "\r\nНа территории есть несколько складов внушительных размеров, котельная, инструментальный цех, депо и непосредственно производственный " +
                    "корпус. Некоторые здания используются с момента основания завода и представляют некоторую историческую ценность. К предприятию подходит ж/д ветка.",
                    YearClosure = "2014",
                    CreatedAt = DateTime.UtcNow.ToString()
                };
                _context.Places.Add(place);
                await _context.SaveChangesAsync();

                // Создаём фото с навигационным свойством
                var image = new Image
                {
                    ImageName = "photoSugarRefiningPlant.jpg",
                    ImagePath = "/uploads/photoSugarRefiningPlant.jpg",
                    IsPrimary = 1,
                    PlaceId = place.Id,
                    Place = place
                };
                _context.Images.Add(image);
                await _context.SaveChangesAsync();

                // Загружаем заново с фото
                places = await _context.Places
                    .Include(p => p.Images)
                    .Include(p => p.City)
                        .ThenInclude(c=>c.Country)
                    .Include(p => p.Category)
                    .ToListAsync();

                _logger.LogInformation("Тестовые данные добавлены");
            }

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
