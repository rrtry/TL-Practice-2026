using Domain;
using Domain.Repositories;
using Domain.Services;
using Infrastructure.Database;
using Infrastructure.Repositories;
using Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.DependencyInjection;

public static class InfrastructureServiceExtensions
{
    public static IServiceCollection AddInfrastructureServices(
        this IServiceCollection services,
        IConfiguration configuration )
    {
        services.AddDbContext<HotelManagementDbContext>( options =>
            options.UseSqlServer( configuration.GetConnectionString( "DefaultConnection" ) ) );

        services.AddScoped<IPropertyRepository, PropertyRepository>();
        services.AddScoped<IRoomTypeRepository, RoomTypeRepository>();
        services.AddScoped<IReservationRepository, ReservationRepository>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        services.AddScoped<IPropertyService, PropertyService>();
        services.AddScoped<IRoomTypeService, RoomTypeService>();
        services.AddScoped<IReservationService, ReservationService>();
        services.AddScoped<ISearchService, SearchService>();

        return services;
    }
}