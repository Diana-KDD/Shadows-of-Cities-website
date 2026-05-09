using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace WebsiteProgect.Models.Configurations
{
    public class PlaceConfigurations : IEntityTypeConfiguration<Place>
    {
        public void Configure(EntityTypeBuilder<Place> builder)
        {
            builder.HasKey(p => p.Id);

            builder.Property(p => p.Name)
                   .HasMaxLength(50)
                   .IsRequired();
            builder.HasIndex(p => p.Name)
                   .IsUnique();

            builder.HasOne(p => p.Category)
                   .WithMany(p => p.Places)
                   .HasForeignKey(p => p.CategoryId)
                   .OnDelete(DeleteBehavior.SetNull);

            builder.HasOne(p => p.City)
                   .WithMany(p => p.Places)
                   .HasForeignKey(p => p.CityId)
                   .OnDelete(DeleteBehavior.SetNull);
        }
    }
}
