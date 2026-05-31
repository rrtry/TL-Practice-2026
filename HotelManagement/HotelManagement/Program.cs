using Domain.Interfaces.Repositories;
using Domain.Interfaces.Services;
using HotelManagement.WebApi.Middleware;
using Infrastructure.Repositories;
using Infrastructure.Services;

var builder = WebApplication.CreateBuilder( args );

builder.Services.AddControllers();
builder.Services.AddOpenApi();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSingleton<IPropertyRepository, InMemoryPropertyRepository>();
builder.Services.AddSingleton<IRoomTypeRepository, InMemoryRoomTypeRepository>();
builder.Services.AddSingleton<IReservationRepository, InMemoryReservationRepository>();

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
}

app.UseMiddleware<ExceptionHandlingMiddleware>();
app.UseAuthorization();
app.MapControllers();
app.Run();

public partial class Program { }