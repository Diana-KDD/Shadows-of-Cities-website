using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebsiteProgect.Data;
using WebsiteProgect.DTO;
using WebsiteProgect.Models;

namespace WebsiteProgect.Controllers
{
    public class AdminController:Controller
    {
        private readonly AppDbContext _context;
        private readonly ILogger<AdminController> _logger;

        private int maxFiles = 7;
        private string[] allowedTypes = new[] { "image/jpeg", "image/png", "image/webp" };
        private int maxSize = 5 * 1024 * 1024; //- 5 МБ в байтах

        public AdminController(AppDbContext context, ILogger<AdminController> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<IActionResult> Index()
        {
            var places = await _context.Places
                    .Include(p => p.Images)
                    .Include(p => p.Category)
                    .Include(p => p.City)
                        .ThenInclude(c => c.Country)
                    .OrderBy(p => p.Name)
                    .ToListAsync();

            return View(places);
        }

        private void FillViewBags()
        {
            ViewBag.Categories = _context.Categories.OrderBy(c => c.Name).ToList();
            ViewBag.Countries = _context.Countries.OrderBy(c => c.Name).ToList();
        }

        [HttpGet]
        public IActionResult Create()
        {
            FillViewBags();
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Place place, List<IFormFile> Image)
        {
            //-- Убираем все ошибки валидации
            ModelState.Remove("Category");
            ModelState.Remove("Category.Name");
            ModelState.Remove("City");
            ModelState.Remove("City.Name");
            ModelState.Remove("City.Country");
            ModelState.Remove("City.Country.Name");
            ModelState.Remove("CreatedAt");

            place.CreatedAt = DateTime.UtcNow.AddHours(3);

            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors);
                foreach (var error in errors)
                    _logger.LogWarning($"Ошибка: {error.ErrorMessage}");
                FillViewBags();
                return View(place);
            }

            if(place.CategoryId == null)
            {
                var categoryOther = _context.Categories.FirstOrDefault(c => c.Name == "Другое");
                if (categoryOther == null)
                {
                    categoryOther = new Category { Name = "Другое", IsDefault=true };
                    _context.Categories.Add(categoryOther);
                    await _context.SaveChangesAsync();
                }
                place.CategoryId = categoryOther.Id;
            }

            _context.Places.Add(place);
            await _context.SaveChangesAsync();

            //-- Фото
            if (Image != null && Image.Count > 0)
            {

                if(Image.Count > maxFiles)
                {
                    ModelState.AddModelError("Image", $"Можно загрузить не более {maxFiles} фото");
                    FillViewBags();
                    return View(place);
                }

                //------- Проверка форматов и размера

                foreach (var file in Image)
                {
                    if (!allowedTypes.Contains(file.ContentType))
                    {
                        ModelState.AddModelError("Image", $"Формат файла {file.FileName} не поддерживается");
                        FillViewBags();
                        return View(place);
                    }

                    if (file.Length > 5 * 1024 * 1024)
                    {
                        ModelState.AddModelError("Image", $"Файл {file.FileName} превышает 5 МБ");
                        FillViewBags();
                        return View(place);
                    }
                }

                var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads");
                if (!Directory.Exists(uploadsFolder))
                    Directory.CreateDirectory(uploadsFolder);

                for (int i = 0; i < Image.Count; i++)
                {
                    var file = Image[i];
                    if (file.Length > 0)
                    {
                        var fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                        var filePath = Path.Combine(uploadsFolder, fileName);

                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            await file.CopyToAsync(stream);
                        }

                        _context.Images.Add(new Image
                        {
                            ImageName = fileName,
                            ImagePath = "/uploads/" + fileName,
                            PlaceId = place.Id,
                            IsPrimary = (i == 0) ? true : false
                        });
                    }
                }
                await _context.SaveChangesAsync();
            }

            _logger.LogInformation($"Добавлено: {place.Name} (ID: {place.Id})");
            return RedirectToAction("Index");
        }

       
        public async Task<IActionResult> Delete(int id)
        {
            var place = await _context.Places
                .Include(p=>p.City)
                    .ThenInclude(c=>c.Country)
                .Include(p => p.Category)
                .Include(p => p.Images)
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

        [HttpGet]
        public async Task<IActionResult> Change(int id)
        {
            FillViewBags();

            var place = await _context.Places
                .Include(p => p.City)
                    .ThenInclude(c => c.Country)
                .Include(p => p.Category)
                .Include(p => p.Images)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (place == null)
                return NotFound();

            return View(place);
        }

        [HttpPost]
        [ActionName("Change")]
        public async Task<IActionResult> ChangeConfirmed(int id, [FromForm]PlaceDto dto, List<IFormFile> Image)
        {
            var place = await _context.Places
                .Include(p => p.City)
                    .ThenInclude(c => c.Country)
                .Include(p => p.Category)
                .Include(p => p.Images)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (place == null)
                return NotFound();

            if(dto == null)
                return BadRequest("Изменения не дошли");

            if(dto.Name != null && place.Name.ToLower() != dto.Name.ToLower())
            {
                var exists = await _context.Places.AnyAsync(p => p.Name == dto.Name);
                if (exists)
                {
                    return BadRequest("Место с таким именем уже есть");
                }
                place.Name = dto.Name;
            }

            if(dto.CategoryId != null &&  place.CategoryId != dto.CategoryId)
            {
                var category = await _context.Categories.FirstOrDefaultAsync(c=>c.Id == dto.CategoryId);
                if(category == null)
                {
                    return BadRequest("Категория не найдена");
                }
                place.Category = category;
                place.CategoryId = dto.CategoryId;
            }

            if (dto.CityId != null && place.CityId != dto.CityId)
            {
                var city = await _context.Cities.FirstOrDefaultAsync(c => c.Id == dto.CityId);
                if (city == null)
                {
                    return BadRequest("Город не найдена");
                }
                place.City = city;
                place.CityId = dto.CityId;
            }

            if(dto.Description != null && place.Description != dto.Description)
            {
                place.Description = dto.Description;
            }

            if (dto.History != null && place.History != dto.History)
            {
                place.History = dto.History;
            }

            if (dto.YearClosure != null && place.YearClosure != dto.YearClosure)
            {
                place.YearClosure = dto.YearClosure;
            }


            //------ Картинки
            if (Image != null && Image.Count > 0)
            {
                if (Image.Count > maxFiles)
                {
                    ModelState.AddModelError("Image", $"Можно загрузить не более {maxFiles} фото");
                    FillViewBags();
                    return View(place);
                }

                //------- Проверка форматов и размера
                foreach (var file in Image)
                {
                    if (!allowedTypes.Contains(file.ContentType))
                    {
                        ModelState.AddModelError("Image", $"Формат файла {file.FileName} не поддерживается");
                        FillViewBags();
                        return View(place);
                    }

                    if (file.Length > maxSize)
                    {
                        ModelState.AddModelError("Image", $"Файл {file.FileName} превышает 5 МБ");
                        FillViewBags();
                        return View(place);
                    }
                }

                var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads");
                if (!Directory.Exists(uploadsFolder))
                    Directory.CreateDirectory(uploadsFolder);

                //-------- Удаляем старые фото с диска и из БД
                foreach (var oldImage in place.Images.ToList())
                {
                    var oldFilePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", oldImage.ImagePath.TrimStart('/'));
                    if (System.IO.File.Exists(oldFilePath))
                    {
                        System.IO.File.Delete(oldFilePath);
                    }
                    _context.Images.Remove(oldImage);
                }
                place.Images.Clear();

                //--------- Сохраняем новые фото
                for (int i = 0; i < Image.Count; i++)
                {
                    var file = Image[i];
                    if (file.Length > 0)
                    {
                        var fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                        var filePath = Path.Combine(uploadsFolder, fileName);

                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            await file.CopyToAsync(stream);
                        }

                        place.Images.Add(new Image
                        {
                            ImageName = fileName,
                            ImagePath = "/uploads/" + fileName,
                            PlaceId = place.Id,
                            IsPrimary = (i == 0)
                        });
                    }
                }
            }

            place.CreatedAt = DateTime.UtcNow.AddHours(3);

            await _context.SaveChangesAsync();

            _logger.LogInformation($"Изменена карточка с местом: {place.Name}");
            return RedirectToAction("Index");
        }
    }
}
