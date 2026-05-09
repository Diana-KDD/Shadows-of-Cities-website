using System.ComponentModel.DataAnnotations.Schema;

namespace WebsiteProgect.Models
{
    public class City
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public int CountryId { get; set; }

        [NotMapped]
        public Country Country { get; set; } = new Country();
        public ICollection<Place> Places { get; set; } = new List<Place>();
    }
}
