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

        [HttpGet("GetCategories")]
        public async Task<IActionResult> GetCategories()
        {
            var categories = await _context.Categories
                .OrderBy(c => c.Name)
                .ToListAsync();
            return Ok(categories);
        }

        [HttpGet("GetCountries")]
        public async Task<IActionResult> GetCountries()
        {
            var countries = await _context.Countries
                .OrderBy(c => c.Name)
                .ToListAsync();
            return Ok(countries);
        }

        [HttpPost("AddCountry")]
        public async Task<IActionResult> AddCountry([FromBody] CountryDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Name) || dto.Name.Length < 2)
            {
                return BadRequest("Название должно быть не менее 2 символов");
            }

            var exists = await _context.Countries.AnyAsync(c => c.Name.ToLower() == dto.Name.ToLower());
            if (exists)
            {
                return Conflict("Страна с таким названием уже существует");
            }

            var country = new Country { Name = dto.Name };
            _context.Countries.Add(country);
            await _context.SaveChangesAsync();

            return Ok(new { id = country.Id, name = country.Name });
        }

        [HttpPut("ChangeCountry/{id}")]
        public async Task<IActionResult> ChangeCountries(int id, [FromBody] CountryDto dto)
        {
            var country = _context.Countries.FirstOrDefault(c => c.Id == id);
            if (country == null)
            {
                return BadRequest("Страна не найдена");
            }

            var exists = await _context.Countries.AnyAsync(c => c.Name.ToLower() == dto.Name.ToLower());
            if (exists)
            {
                return Conflict("Категория с таким названием уже существует");
            }

            country.Name = dto.Name;
            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpGet("GetCitiesByCountry/{id}")]
        public async Task<IActionResult> GetCitiesByCountry(int countryId)
        {
            var cities = await _context.Cities
                .Where(c => c.CountryId == countryId)
                .OrderBy(c => c.Name)
                .ToListAsync();
            return Ok(cities);
        }

        [HttpGet("GetCities")]
        public async Task<IActionResult> GetCities()
        {
            var cities = await _context.Cities
                        .Include(c => c.Country)
                        .OrderBy(c => c.Country.Name)
                        .ThenBy(c => c.Name)
                        .Select(c => new CityDto
                        {
                            Id = c.Id,
                            Name = c.Name,
                            CountryId = c.CountryId,
                            CountryName = c.Country.Name
                        })
                        .ToListAsync();

            return Ok(cities);
        }

        [HttpPost("AddCity")]
        public async Task<IActionResult> AddCity([FromBody] CityDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Name) || dto.Name.Length < 2)
            {
                return BadRequest("Название должно быть не менее 2 символов");
            }

            var exists = await _context.Cities.AnyAsync(c => c.Name.ToLower() == dto.Name.ToLower());
            if (exists)
            {
                return Conflict("Город с таким названием уже существует");
            }
            if(dto.CountryId == null)
            {
                return BadRequest("Страна не зафиксирована");
            }
            var country = await _context.Countries.FindAsync(dto.CountryId);
            var city = new City { Name = dto.Name, CountryId = (int)dto.CountryId, Country = country };
            _context.Cities.Add(city);
            await _context.SaveChangesAsync();

            return Ok(new { id = city.Id, name = city.Name });
        }

        [HttpPut("ChangeCity/{id}")]
        public async Task<IActionResult> ChangeCities(int id, [FromBody] CityDto dto)
        {
            var city = _context.Cities.FirstOrDefault(c => c.Id == id);
            if (city == null)
            {
                return BadRequest("Город не найден");
            }

            if (city.CountryId != dto.CountryId)
            {
                var country = await _context.Countries.FindAsync(dto.CountryId);
                if (country == null)
                {
                    return BadRequest("Страна не найдена");
                }
                city.CountryId = dto.CountryId;
                city.Country = country;
            }
            
            if (city.Name != dto.Name)
            {
                var exists = await _context.Cities.AnyAsync(c => c.Name.ToLower() == dto.Name.ToLower());
                if (exists)
                {
                    return Conflict("Город с таким названием уже существует");
                }
            }

            city.Name = dto.Name;
            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpPost("AddCategory")]
        public async Task<IActionResult> AddCategory([FromBody] CategoryDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Name) || dto.Name.Length < 2)
            {
                return BadRequest("Название должно быть не менее 2 символов");
            }

            var exists = await _context.Categories.AnyAsync(c => c.Name.ToLower() == dto.Name.ToLower());
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

            var exists = await _context.Categories.AnyAsync(c => c.Name.ToLower() == dto.Name.ToLower());
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

        [HttpDelete("DeleteCountry/{id}")]
        public async Task<IActionResult> DeleteCountry(int id)
        {
            var country = _context.Countries.FirstOrDefault(c => c.Id == id);
            if (country == null)
            {
                return BadRequest("Страна не найдена");
            }
            _context.Countries.Remove(country);
            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpDelete("DeleteCity/{id}")]
        public async Task<IActionResult> DeleteCity(int id)
        {
            var city = _context.Cities.FirstOrDefault(c => c.Id == id);
            if (city == null)
            {
                return BadRequest("Город не найдена");
            }
            _context.Cities.Remove(city);
            await _context.SaveChangesAsync();
            return Ok();
        }

    }
}
