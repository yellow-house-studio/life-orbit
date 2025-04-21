using YellowHouseStudio.LifeOrbit.Application.Data;
using Microsoft.EntityFrameworkCore;
using YellowHouseStudio.LifeOrbit.Api.Infrastructure.Filters;
using YellowHouseStudio.LifeOrbit.Application;
using Serilog;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using HealthChecks.UI.Client;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Npgsql;
using Serilog.Events;

namespace YellowHouseStudio.LifeOrbit.Api;

public class Program
{
    public static void Main(string[] args)
    {
        // Configure Serilog first
        Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Information()
            .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
            .Enrich.FromLogContext()
            .WriteTo.Console()
            .CreateLogger();

        try
        {
            var app = CreateApp(args);
            
            // Configure Kestrel to listen on port 80
            app.Urls.Add("http://[::]:80");
            
            app.Run();
        }
        catch (Exception ex)
        {
            Log.Fatal(ex, "Application start-up failed");
        }
        finally
        {
            Log.CloseAndFlush();
        }
    }

    public static WebApplication CreateApp(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add Serilog to the container
        builder.Host.UseSerilog();
        
        // Register ILogger explicitly
        builder.Services.AddSingleton(Log.Logger);

        // Configure CORS
        builder.Services.AddCors(options =>
        {
            options.AddDefaultPolicy(policy =>
            {
                policy.WithOrigins("http://localhost:4200")
                    .AllowAnyHeader()
                    .AllowAnyMethod();
            });
        });

        // Add services to the container.
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

        // Add NpgsqlDataSource as a singleton
        builder.Services.AddNpgsqlDataSource(builder.Configuration.GetConnectionString("DefaultConnection")!);

        // Add health checks
        builder.Services.AddHealthChecks()
            .AddCheck("postgres", () =>
            {
                try
                {
                    using var dataSource = builder.Services.BuildServiceProvider().GetRequiredService<NpgsqlDataSource>();
                    using var conn = dataSource.CreateConnection();
                    conn.Open();
                    using var cmd = conn.CreateCommand();
                    cmd.CommandText = "SELECT 1";
                    cmd.ExecuteScalar();
                    return HealthCheckResult.Healthy();
                }
                catch (Exception ex)
                {
                    return HealthCheckResult.Unhealthy(ex.Message);
                }
            }, new[] { "db", "sql", "postgres" })
            .AddCheck("self", () => HealthCheckResult.Healthy(), 
                tags: new[] { "service" });

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();
        
        // Use CORS before authorization
        app.UseCors();
        
        app.UseAuthorization();

        // Add health check endpoints
        app.MapHealthChecks("/health", new HealthCheckOptions
        {
            ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
        });
        
        app.MapHealthChecks("/health/ready", new HealthCheckOptions
        {
            Predicate = healthCheck => healthCheck.Tags.Contains("service") || healthCheck.Tags.Contains("db"),
            ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
        });
        
        app.MapHealthChecks("/health/live", new HealthCheckOptions
        {
            Predicate = _ => false,
            ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
        });

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
