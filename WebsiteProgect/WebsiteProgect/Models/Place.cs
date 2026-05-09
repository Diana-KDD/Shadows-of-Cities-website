using System.ComponentModel.DataAnnotations.Schema;

namespace WebsiteProgect.Models
{
    public class Place
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public int? CategoryId { get; set; }
        public int? CityId { get; set; }
        public string Description {  get; set; } = null!;
        public string? History {  get; set; }
        public string? YearClosure { get; set; }
        public string CreatedAt { get; set; } = null!;

        [NotMapped]
        public Category? Category { get; set; } = new Category();
        [NotMapped]
        public City? City { get; set; } = new City();

        public ICollection<Image> Images { get; set; } = new List<Image>();

    }
}
