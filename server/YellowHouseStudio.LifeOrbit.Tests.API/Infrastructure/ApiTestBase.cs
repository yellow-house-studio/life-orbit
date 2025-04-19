using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NUnit.Framework;
using Serilog;
using Serilog.Events;
using YellowHouseStudio.LifeOrbit.Api;
using YellowHouseStudio.LifeOrbit.Api.Infrastructure.Filters;
using YellowHouseStudio.LifeOrbit.Application;
using YellowHouseStudio.LifeOrbit.Application.Data;

namespace YellowHouseStudio.LifeOrbit.Tests.API.Infrastructure;

public abstract class ApiTestBase
{
    protected WebApplicationFactory<Program> Factory = null!;
    protected HttpClient Client = null!;
    protected static Serilog.ILogger Logger = new LoggerConfiguration()
        .MinimumLevel.Debug()
        .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
        .MinimumLevel.Override("Microsoft.AspNetCore", LogEventLevel.Error)
        .Enrich.FromLogContext()
        .WriteTo.Console(
            outputTemplate: "[{Timestamp:HH:mm:ss} {Level:u3}] {Message:lj} {Properties:j}{NewLine}{Exception}")
        .CreateLogger();

    [OneTimeSetUp]
    public void BaseSetup()
    {
        // Set static logger for test run
        Log.Logger = Logger;
        
        Factory = new WebApplicationFactory<Program>()
            .WithWebHostBuilder(builder =>
            {
                builder.ConfigureServices(services =>
                {
                    // Remove the app's ApplicationDbContext registration
                    var descriptor = services.SingleOrDefault(
                        d => d.ServiceType == typeof(DbContextOptions<ApplicationDbContext>));

                    if (descriptor != null)
                    {
                        services.Remove(descriptor);
                    }

                    // Add ApplicationDbContext using an in-memory database for testing
                    services.AddDbContext<ApplicationDbContext>(options =>
                    {
                        options.UseInMemoryDatabase("TestDb")
                              .ConfigureWarnings(warnings => 
                                  warnings.Ignore(Microsoft.EntityFrameworkCore.Diagnostics.InMemoryEventId.TransactionIgnoredWarning));
                    });

                    // Add Application layer with all behaviors
                    services.AddApplication();

                    // Add Serilog logger
                    services.AddSingleton<Serilog.ILogger>(Logger);
                    services.AddSingleton(Logger); // Also register as Serilog.ILogger
                });

                builder.ConfigureLogging(loggingBuilder =>
                {
                    loggingBuilder.ClearProviders();
                    loggingBuilder.AddSerilog(Logger, dispose: true);
                });
            });
            
        var testServer = Factory.Server;
        testServer.PreserveExecutionContext = true; // This is needed to get the logs from inside of the API to be captured by the test and shown
        Client = Factory.CreateClient();
    }

    [OneTimeTearDown]
    public void BaseTearDown()
    {
        Client.Dispose();
        Factory.Dispose();
        Log.CloseAndFlush(); // Ensure all logs are flushed
    }
}

public class TestWebHostEnvironment : IWebHostEnvironment
{
    public string WebRootPath { get; set; } = string.Empty;
    public IFileProvider WebRootFileProvider { get; set; } = new NullFileProvider();
    public string ApplicationName { get; set; } = "Test Application";
    public IFileProvider ContentRootFileProvider { get; set; } = new NullFileProvider();
    public string ContentRootPath { get; set; } = Directory.GetCurrentDirectory();
    public string EnvironmentName { get; set; } = Environments.Development;
} 