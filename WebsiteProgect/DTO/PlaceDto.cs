using WebsiteProgect.Models;

namespace WebsiteProgect.DTO
{
    public class PlaceDto
    {
        public string Name { get; set; } = null!;
        public int? CategoryId { get; set; }
        public int? CityId { get; set; }
        public string Description { get; set; } = null!;
        public string? History { get; set; }
        public string? YearClosure { get; set; }
    }
}
