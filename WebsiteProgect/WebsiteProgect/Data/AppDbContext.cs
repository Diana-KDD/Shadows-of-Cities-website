using Microsoft.EntityFrameworkCore;
using WebsiteProgect.Models;
using WebsiteProgect.Models.Configurations;

namespace WebsiteProgect.Data
{
    public class AppDbContext:DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }
        public DbSet<Country> Countries { get; set; } = null!;
        public DbSet<City> Cities { get; set; } = null!;
        public DbSet<Category> Categories { get; set; } = null!;
        public DbSet<Place> Places { get; set; } = null!;
        public DbSet<Image> Images { get; set; } = null!;
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfiguration(new CountryConfigurations());
            modelBuilder.ApplyConfiguration(new CityConfigurations());
            modelBuilder.ApplyConfiguration(new CategoryConfigurations());
            modelBuilder.ApplyConfiguration(new PlaceConfigurations());
            modelBuilder.ApplyConfiguration(new ImageConfigurations());
        }
    }
}
