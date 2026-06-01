using Domain.Interfaces;
using Domain.Interfaces.Repositories;
using Domain.Interfaces.Services;
using HotelManagement.WebApi.Middleware;
using Infrastructure.Database;
using Infrastructure.Repositories;
using Infrastructure.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder( args );

builder.Services.AddControllers();
builder.Services.AddOpenApi();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<AppDbContext>( options =>
    options.UseSqlServer( builder.Configuration.GetConnectionString( "DefaultConnection" ) ) );

builder.Services.AddScoped<IPropertyRepository, EfPropertyRepository>();
builder.Services.AddScoped<IRoomTypeRepository, EfRoomTypeRepository>();
builder.Services.AddScoped<IReservationRepository, EfReservationRepository>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>(); // UOW

builder.Services.AddScoped<IPropertyService, PropertyService>();
builder.Services.AddScoped<IRoomTypeService, RoomTypeService>();
builder.Services.AddScoped<IReservationService, ReservationService>();
builder.Services.AddScoped<ISearchService, SearchService>();

var app = builder.Build();

if ( app.Environment.IsDevelopment() )
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI();

    using var scope = app.Services.CreateScope();
    var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    await DbInitializer.SeedAsync( dbContext );
}

app.UseMiddleware<ExceptionHandlingMiddleware>();
app.UseAuthorization();
app.MapControllers();
app.Run();

public partial class Program { }