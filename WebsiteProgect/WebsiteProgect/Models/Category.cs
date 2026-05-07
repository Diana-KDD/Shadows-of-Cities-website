namespace WebsiteProgect.Models
{
    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public ICollection<Places> Places { get; set; } = new List<Places>();
    }
}
