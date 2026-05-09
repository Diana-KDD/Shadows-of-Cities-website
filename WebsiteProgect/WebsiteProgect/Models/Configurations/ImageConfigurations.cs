using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace WebsiteProgect.Models.Configurations
{
    public class ImageConfigurations : IEntityTypeConfiguration<Image>
    {
        public void Configure(EntityTypeBuilder<Image> builder)
        {
            builder.HasKey(i => i.Id);

            builder.Property(i => i.ImageName)
                   .HasMaxLength(50)
                   .IsRequired();
            builder.HasIndex(i => i.ImageName)
                   .IsUnique();

            builder.HasOne(i => i.Place)
                   .WithMany(i => i.Images)
                   .HasForeignKey(i => i.PlaceId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.Property(i => i.IsPrimary).HasDefaultValue(0);
        }
    }
}
