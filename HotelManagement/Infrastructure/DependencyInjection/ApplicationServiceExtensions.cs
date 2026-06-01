using Domain.Interfaces.Services;
using Infrastructure.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.DependencyInjection;

public static class ApplicationServiceExtensions
{
    public static IServiceCollection AddApplicationServices( this IServiceCollection services )
    {
        services.AddScoped<IPropertyService, PropertyService>();
        services.AddScoped<IRoomTypeService, RoomTypeService>();
        services.AddScoped<IReservationService, ReservationService>();
        services.AddScoped<ISearchService, SearchService>();

        return services;
    }
}