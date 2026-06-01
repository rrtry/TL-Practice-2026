using HotelManagement.WebApi.Middleware;
using Infrastructure.Database;
using Infrastructure.DependencyInjection;

var builder = WebApplication.CreateBuilder( args );

builder.Services.AddControllers();
builder.Services.AddOpenApi();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddInfrastructure( builder.Configuration );
builder.Services.AddApplicationServices();

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