using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace WebsiteProgect.Models.Configurations
{
    public class CountryConfigurations : IEntityTypeConfiguration<Country>
    {
        public void Configure(EntityTypeBuilder<Country> builder)
        {
            builder.HasKey(c=>c.Id);

            builder.Property(c=>c.Name)
                   .HasMaxLength(50);
            builder.HasIndex(c=>c.Name)
                   .IsUnique();
        }
    }
}
