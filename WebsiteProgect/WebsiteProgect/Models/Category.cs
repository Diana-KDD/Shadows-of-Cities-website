namespace WebsiteProgect.Models
{
    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public bool IsDefault { get; set; } = false;
        public ICollection<Place> Places { get; set; } = new List<Place>();
    }
}
