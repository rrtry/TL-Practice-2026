using Domain.Interfaces.Repositories;
using Infrastructure.Repositories;

using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;

namespace HotelManagement.Tests;

public class CustomWebApplicationFactory : WebApplicationFactory<Program>
{
    protected override void ConfigureWebHost( IWebHostBuilder builder )
    {
        builder.ConfigureServices( services =>
        {
            RemoveServiceIfExists( services, typeof( IPropertyRepository ) );
            RemoveServiceIfExists( services, typeof( IRoomTypeRepository ) );
            RemoveServiceIfExists( services, typeof( IReservationRepository ) );

            services.AddSingleton<IPropertyRepository, InMemoryPropertyRepository>();
            services.AddSingleton<IRoomTypeRepository, InMemoryRoomTypeRepository>();
            services.AddSingleton<IReservationRepository, InMemoryReservationRepository>();
        } );
    }

    private void RemoveServiceIfExists( IServiceCollection services, Type serviceType )
    {
        var service = services.SingleOrDefault( d => d.ServiceType == serviceType );
        if ( service != null ) services.Remove( service );
    }
}