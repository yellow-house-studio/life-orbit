using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using YellowHouseStudio.LifeOrbit.Application.Configuration;
using YellowHouseStudio.LifeOrbit.Application.Data;
using Microsoft.Extensions.Logging;
using MediatR;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace YellowHouseStudio.LifeOrbit.Tests.Integration;

public abstract class TestBase
{
    protected ApplicationDbContext Context { get; private set; } = null!;
    protected IMediator Mediator { get; private set; } = null!;
    protected IServiceProvider ServiceProvider { get; private set; } = null!;

    [SetUp]
    public virtual void BaseSetup()
    {
        var services = new ServiceCollection();

        // Database setup
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .ConfigureWarnings(warnings => warnings.Ignore(InMemoryEventId.TransactionIgnoredWarning))
            .Options;

        Context = new ApplicationDbContext(options);
        services.AddSingleton(Context);

        // Logging
        services.AddLogging(builder => builder.AddConsole());

        // MediatR and Application Layer setup
        services.AddApplicationMediatR();

        // Build service provider
        ServiceProvider = services.BuildServiceProvider();
        Mediator = ServiceProvider.GetRequiredService<IMediator>();
    }

    [TearDown]
    public virtual void BaseTearDown()
    {
        Context.Database.EnsureDeleted();
        Context.Dispose();
        if (ServiceProvider is IDisposable disposable)
        {
            disposable.Dispose();
        }
    }
} 