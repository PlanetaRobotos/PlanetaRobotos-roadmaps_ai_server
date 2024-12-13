using Fleet.Core.Constants;
using Fleet.Core.Types;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fleet.Domain.OwnedTypes;

[Owned]
public class Address : ITableSettingsIgnore
{
    public string Text { get; set; } = null!;

    public double Latitude { get; init; }
    public double Longitude { get; init; }
    public string? GooglePlaceId { get; set; }


    public static void ConfigureAsOptionalOwnedEntity<T>(OwnedNavigationBuilder<T, Address> builder)
        where T : class
    {
        builder.Property(e => e.Text).HasColumnName("Address").HasMaxLength(StringLimits._500);
        builder.Property(e => e.Latitude).HasColumnName(nameof(Latitude));
        builder.Property(e => e.Longitude).HasColumnName(nameof(Longitude));
        builder.Property(e => e.GooglePlaceId).HasColumnName("AddressGoogleId").HasMaxLength(StringLimits._100);
    }

    public static void ConfigureAsRequiredOwnedEntity<T>(OwnedNavigationBuilder<T, Address> builder)
        where T : class
    {
        builder.Property(e => e.Text).HasColumnName("Address").IsRequired().HasMaxLength(StringLimits._500);
        builder.Property(e => e.Latitude).HasColumnName(nameof(Latitude)).IsRequired();
        builder.Property(e => e.Longitude).HasColumnName(nameof(Longitude)).IsRequired();
        builder.Property(e => e.GooglePlaceId).HasColumnName("AddressGoogleId").HasMaxLength(StringLimits._100);
    }
}