using System.ComponentModel.DataAnnotations.Schema;

namespace WebsiteProgect.Models
{
    public class City
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public int CountryId { get; set; }

        public Country? Country { get; set; }
        public ICollection<Place> Places { get; set; } = new List<Place>();
    }
}
