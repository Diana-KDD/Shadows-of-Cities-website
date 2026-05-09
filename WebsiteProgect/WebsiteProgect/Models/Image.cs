using System.ComponentModel.DataAnnotations.Schema;

namespace WebsiteProgect.Models
{
    public class Image
    {
        public int Id { get; set; }
        public string ImageName { get; set; } = null!;
        public string ImagePath { get; set; } = null!;
        public int PlaceId { get; set; }
        public int IsPrimary {  get; set; }

        [NotMapped]
        public Place Place { get; set; } = new Place();
    }
}
