using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebsiteProgect.Data;
using WebsiteProgect.DTO;
using WebsiteProgect.Models;

namespace WebsiteProgect.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ApiController:ControllerBase
    {
        private readonly AppDbContext _context;

        public ApiController(AppDbContext context)
        {  
            _context = context;
        }

        [HttpGet("GetCities")]
        public async Task<IActionResult> GetCities(int countryId)
        {
            var cities = await _context.Cities
                .Where(c=>c.CountryId == countryId)
                .OrderBy(c=>c.Name)
                .ToListAsync();
            return Ok(cities);
        }

        [HttpGet("GetCategories")]
        public async Task<IActionResult> GetCategories()
        {
            var categories = await _context.Categories
                .OrderBy(c => c.Name)
                .ToListAsync();
            return Ok(categories);
        }

        [HttpPost("AddCategory")]
        public async Task<IActionResult> AddCategory([FromBody] CategoryDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Name) || dto.Name.Length < 2)
            {
                return BadRequest("Название должно быть не менее 2 символов");
            }

            var exists = await _context.Categories.AnyAsync(c => c.Name == dto.Name);
            if (exists)
            {
                return Conflict("Категория с таким названием уже существует");
            }

            var category = new Category { Name = dto.Name };
            _context.Categories.Add(category);
            await _context.SaveChangesAsync();

            return Ok(new { id = category.Id, name = category.Name });
        }

        [HttpPut("ChangeCategory/{id}")]
        public async Task<IActionResult> ChangeCategory(int id, [FromBody] CategoryDto dto)
        {
            var category = _context.Categories.FirstOrDefault(c => c.Id == id);
            if (category == null)
            {
                return BadRequest("Категория не найдена");
            }

            var exists = await _context.Categories.AnyAsync(c => c.Name == dto.Name);
            if (exists)
            {
                return Conflict("Категория с таким названием уже существует");
            }

            category.Name = dto.Name;
            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpDelete("DeleteCategory/{id}")]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            var category = _context.Categories.FirstOrDefault(c => c.Id == id);
            if (category == null)
            {
                return BadRequest("Категория не найдена");
            }
            _context.Categories.Remove(category);
            await _context.SaveChangesAsync();
            return Ok();
        }

    }
}
