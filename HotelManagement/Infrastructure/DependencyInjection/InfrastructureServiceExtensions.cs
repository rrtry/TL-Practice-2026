using Domain;
using Domain.Repositories;
using Infrastructure.Database;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.DependencyInjection;

public static class InfrastructureServiceExtensions
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration )
    {
        services.AddDbContext<AppDbContext>( options =>
            options.UseSqlServer( configuration.GetConnectionString( "DefaultConnection" ) ) );

        services.AddScoped<IPropertyRepository, EfPropertyRepository>();
        services.AddScoped<IRoomTypeRepository, EfRoomTypeRepository>();
        services.AddScoped<IReservationRepository, EfReservationRepository>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        return services;
    }
}