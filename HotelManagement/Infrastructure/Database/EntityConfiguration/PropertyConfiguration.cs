using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Database.EntityConfiguration;

public class PropertyConfiguration : IEntityTypeConfiguration<Property>
{
    public void Configure( EntityTypeBuilder<Property> builder )
    {
        builder.HasKey( p => p.Id );

        builder.ToTable( nameof( Property ) );

        builder.Property( p => p.Id )
            .HasDefaultValueSql( "NEWSEQUENTIALID()" )
            .ValueGeneratedOnAdd();

        builder.Property( p => p.Name )
            .IsRequired()
            .HasMaxLength( 255 );

        builder.Property( p => p.Country )
            .IsRequired()
            .HasMaxLength( 255 );

        builder.Property( p => p.City )
            .IsRequired()
            .HasMaxLength( 255 );

        builder.Property( p => p.Address )
            .IsRequired()
            .HasMaxLength( 255 );
    }
}