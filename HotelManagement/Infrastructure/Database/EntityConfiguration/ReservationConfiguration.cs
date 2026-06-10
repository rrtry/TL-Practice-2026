using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Database.EntityConfiguration;

public class ReservationConfiguration : IEntityTypeConfiguration<Reservation>
{
    public void Configure( EntityTypeBuilder<Reservation> builder )
    {
        builder.HasKey( r => r.Id );

        builder.ToTable( nameof( Reservation ) );

        builder.Property( r => r.ArrivalDate )
            .HasColumnType( "date" )
            .IsRequired();

        builder.Property( r => r.DepartureDate )
            .HasColumnType( "date" )
            .IsRequired();

        builder.Property( r => r.ArrivalTime )
            .HasColumnType( "time" )
            .IsRequired();

        builder.Property( r => r.DepartureTime )
            .HasColumnType( "time" )
            .IsRequired();

        builder.Property( r => r.GuestName )
            .IsRequired()
            .HasMaxLength( 255 );

        builder.Property( r => r.GuestPhoneNumber )
            .IsRequired()
            .HasMaxLength( 30 );

        builder.Property( r => r.Total )
            .HasColumnType( "decimal(18,2)" )
            .IsRequired();

        builder.Property( r => r.Currency )
            .HasMaxLength( 10 )
            .IsRequired();

        builder.Property( r => r.GuestCount )
            .IsRequired();

        builder.HasOne<Property>()
            .WithMany()
            .HasForeignKey( r => r.PropertyId )
            .OnDelete( DeleteBehavior.Restrict )
            .IsRequired();

        builder.HasOne<RoomType>()
            .WithMany()
            .HasForeignKey( r => r.RoomTypeId )
            .OnDelete( DeleteBehavior.Restrict )
            .IsRequired();
    }
}