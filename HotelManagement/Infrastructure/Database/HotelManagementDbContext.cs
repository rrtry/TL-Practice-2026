using Domain.Entities;
using Infrastructure.Database.EntityConfiguration;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Database;

public class HotelManagementDbContext : DbContext
{
    public HotelManagementDbContext( DbContextOptions<HotelManagementDbContext> options ) : base( options ) { }

    public DbSet<Property> Properties => Set<Property>();
    public DbSet<RoomType> RoomTypes => Set<RoomType>();
    public DbSet<Reservation> Reservations => Set<Reservation>();

    protected override void OnModelCreating( ModelBuilder modelBuilder )
    {
        modelBuilder.ApplyConfiguration( new PropertyConfiguration() );
        modelBuilder.ApplyConfiguration( new ReservationConfiguration() );
        modelBuilder.ApplyConfiguration( new RoomTypeConfiguration() );
    }
}