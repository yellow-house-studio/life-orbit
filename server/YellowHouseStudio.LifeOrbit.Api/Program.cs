using YellowHouseStudio.LifeOrbit.Application.Data;
using Microsoft.EntityFrameworkCore;
using YellowHouseStudio.LifeOrbit.Api.Infrastructure.Filters;
using YellowHouseStudio.LifeOrbit.Application;

namespace YellowHouseStudio.LifeOrbit.Api;

public class Program
{
    public static void Main(string[] args)
    {
        var app = CreateApp(args);
        app.Run();
    }

    public static WebApplication CreateApp(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddControllers(options =>
        {
            // Order matters! GlobalExceptionFilter must be registered first to be the first and last filter to be executed
            options.Filters.Add<GlobalExceptionFilter>();
            options.Filters.Add<ValidationExceptionFilter>();
        
        });
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        // Add Application Layer
        builder.Services.AddApplication();

        // Add DbContext
        builder.Services.AddDbContext<ApplicationDbContext>(options =>
            options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();
        app.UseAuthorization();
        app.MapControllers();

        var summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        app.MapGet("/weatherforecast", () =>
        {
            var forecast =  Enumerable.Range(1, 5).Select(index =>
                new WeatherForecast
                (
                    DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                    Random.Shared.Next(-20, 55),
                    summaries[Random.Shared.Next(summaries.Length)]
                ))
                .ToArray();
            return forecast;
        })
        .WithName("GetWeatherForecast")
        .WithOpenApi();

        return app;
    }
}

record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}
