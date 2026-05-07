using Microsoft.EntityFrameworkCore;
using WebsiteProgect.Models;
using WebsiteProgect.Models.Configurations;

namespace WebsiteProgect.Data
{
    public class AppDbContext:DbContext
    {
        public DbSet<Country> Countries { get; set; } = null!;
        public DbSet<City> Cities { get; set; } = null!;
        public DbSet<Category> Categories { get; set; } = null!;
        public DbSet<Places> Places { get; set; } = null!;
        public DbSet<Image> Images { get; set; } = null!;
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            optionsBuilder.UseSqlServer("Data Source=HOME-PC\\MSSQLSERVER01;Initial Catalog=WebSiteShadowsOfCities;Integrated Security=True;Connect Timeout=30;Encrypt=True;Trust Server Certificate=True;Application Intent=ReadWrite;Multi Subnet Failover=False");
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfiguration(new CityConfigurations());
            modelBuilder.ApplyConfiguration(new CategoryConfigurations());
            modelBuilder.ApplyConfiguration(new PlacesConfigurations());
            modelBuilder.ApplyConfiguration(new ImageConfigurations());
        }
    }
}
