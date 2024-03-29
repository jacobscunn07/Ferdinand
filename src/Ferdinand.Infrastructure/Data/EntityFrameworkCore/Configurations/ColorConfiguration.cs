using Ferdinand.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Ferdinand.Infrastructure.Data.EntityFrameworkCore.Configurations;

public sealed class ColorConfiguration : IEntityTypeConfiguration<Color>
{
    public void Configure(EntityTypeBuilder<Color> builder)
    {
        builder.ToTable("Color");

        builder.HasKey(x => x.Key);

        builder.Property(x => x.Key)
        .ValueGeneratedNever()
        .HasConversion(
            id => id.Value,
            value => ColorKey.Create(value)
        );

        builder.Property(x => x.Tenant)
        .HasConversion(
            id => id.Value,
            value => Tenant.Create(value)
        )
        .IsRequired()
        .HasMaxLength(50);

        builder.Property(x => x.HexValue)
        .IsRequired()
        .HasMaxLength(6);

        builder.HasIndex(x => new { x.Tenant, x.HexValue }).IsUnique();

        builder.Property(x => x.Description)
        .HasMaxLength(100);
    }
}
