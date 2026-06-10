using Domain;
using Domain.Repositories;
using HotelManagement.Tests.Fakes;
using HotelManagement.Tests.Repositories;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace HotelManagement.Tests;

public class CustomWebApplicationFactory : WebApplicationFactory<Program>
{
    protected override void ConfigureWebHost( IWebHostBuilder builder )
    {
        builder.UseEnvironment( "Test" ); // Явно устанавливаем среду, т.к отсутствует AppDbContext

        builder.ConfigureServices( services =>
        {
            RemoveServiceByType( services, typeof( IPropertyRepository ) );
            RemoveServiceByType( services, typeof( IRoomTypeRepository ) );
            RemoveServiceByType( services, typeof( IReservationRepository ) );

            var dbContextDescriptor = services.SingleOrDefault(
                d => d.ServiceType == typeof( DbContextOptions<Infrastructure.Database.AppDbContext> ) );

            if ( dbContextDescriptor != null )
            {
                services.Remove( dbContextDescriptor );
            }

            RemoveServiceByType( services, typeof( Infrastructure.Database.AppDbContext ) );
            RemoveServiceByType( services, typeof( IUnitOfWork ) );

            services.AddSingleton<IPropertyRepository, InMemoryPropertyRepository>();
            services.AddSingleton<IRoomTypeRepository, InMemoryRoomTypeRepository>();
            services.AddSingleton<IReservationRepository, InMemoryReservationRepository>();
            services.AddSingleton<IUnitOfWork, FakeUnitOfWork>();

        } );
    }

    private void RemoveServiceByType( IServiceCollection services, Type serviceType )
    {
        var descriptor = services.SingleOrDefault( d => d.ServiceType == serviceType );
        if ( descriptor != null )
        {
            services.Remove( descriptor );
        }
    }
}