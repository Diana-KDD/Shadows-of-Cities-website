using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace WebsiteProgect.Models.Configurations
{
    public class CityConfigurations : IEntityTypeConfiguration<City>
    {
        public void Configure(EntityTypeBuilder<City> builder)
        {
            builder.HasKey(c => c.Id);

            builder.Property(c => c.Name)
                   .HasMaxLength(50)
                   .IsRequired();
            builder.HasIndex(c=>c.Name)
                   .IsUnique();

            builder.HasOne(c=>c.Country)
                   .WithMany(c=>c.Cities)
                   .HasForeignKey(c=>c.CountryId)
                   .OnDelete(DeleteBehavior.Cascade);

        }
    }
}
