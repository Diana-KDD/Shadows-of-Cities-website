using System.ComponentModel.DataAnnotations.Schema;

namespace WebsiteProgect.Models
{
    public class Image
    {
        public int Id { get; set; }
        public string ImageName { get; set; } = null!;
        public string ImagePath { get; set; } = null!;
        public int PlacesId { get; set; }
        public int IsPrimary {  get; set; }

        [NotMapped]
        public Places Place { get; set; } = new Places();
    }
}
