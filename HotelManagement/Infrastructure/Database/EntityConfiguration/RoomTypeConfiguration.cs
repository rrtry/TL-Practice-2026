using System.Text.Json;
using Domain.Entities;
using Infrastructure.Database.Comparers;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Database.EntityConfiguration;

public class RoomTypeConfiguration : IEntityTypeConfiguration<RoomType>
{
    public void Configure( EntityTypeBuilder<RoomType> builder )
    {
        builder.HasKey( rt => rt.Id );

        builder.Property( rt => rt.Name )
            .IsRequired()
            .HasMaxLength( 255 );

        builder.Property( rt => rt.DailyPrice )
            .HasColumnType( "decimal(18,2)" )
            .IsRequired();

        builder.Property( rt => rt.Currency )
            .HasMaxLength( 10 )
            .IsRequired();

        builder.Property( rt => rt.MinPersonCount ).IsRequired();
        builder.Property( rt => rt.MaxPersonCount ).IsRequired();
        builder.Property( rt => rt.AvailableRoomsCount ).IsRequired();

        // List<string> в JSON
        var jsonOptions = new JsonSerializerOptions();
        builder.Property( rt => rt.Services )
            .HasConversion(
                v => JsonSerializer.Serialize( v, jsonOptions ),
                v => JsonSerializer.Deserialize<List<string>>( v, jsonOptions ) ?? new List<string>(),
                ListStringComparer.Instance
            )
            .HasColumnType( "nvarchar(max)" );

        builder.Property( rt => rt.Amenities )
            .HasConversion(
                v => JsonSerializer.Serialize( v, jsonOptions ),
                v => JsonSerializer.Deserialize<List<string>>( v, jsonOptions ) ?? new List<string>(),
                ListStringComparer.Instance
            )
            .HasColumnType( "nvarchar(max)" );

        builder.HasOne<Property>()
            .WithMany()
            .HasForeignKey( rt => rt.PropertyId )
            .OnDelete( DeleteBehavior.Restrict )
            .IsRequired();
    }
}