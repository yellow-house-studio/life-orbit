using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using YellowHouseStudio.LifeOrbit.Application.Configuration;
using YellowHouseStudio.LifeOrbit.Application.Data;
using YellowHouseStudio.LifeOrbit.Application.Family.Common;
using YellowHouseStudio.LifeOrbit.Application.Users;
using YellowHouseStudio.LifeOrbit.Infrastructure.Repositories;
using Microsoft.Extensions.Logging;
using MediatR;
using Microsoft.EntityFrameworkCore.Diagnostics;
using YellowHouseStudio.LifeOrbit.Infrastructure.Configuraiton;

namespace YellowHouseStudio.LifeOrbit.Tests.Integration;

public abstract class TestBase
{
    protected ApplicationDbContext Context { get; private set; } = null!;
    protected IMediator Mediator { get; private set; } = null!;
    protected IServiceProvider ServiceProvider { get; private set; } = null!;
    protected ICurrentUser CurrentUser { get; private set; } = null!;

    [SetUp]
    public virtual async Task BaseSetup()
    {
        var services = new ServiceCollection();

        // Database setup
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .ConfigureWarnings(warnings => warnings.Ignore(InMemoryEventId.TransactionIgnoredWarning))
            .Options;

        Context = new ApplicationDbContext(options);
        services.AddSingleton(Context);

        // Current user setup
        CurrentUser = new TestCurrentUser();
        services.AddSingleton<ICurrentUser>(CurrentUser);

        // Repository setup
        services.AddScoped<IFamilyMemberRepository, FamilyMemberRepository>();

        // Logging
        services.AddLogging(builder => builder.AddConsole());

        // MediatR and Application Layer setup
        services.AddApplicationMediatR();

        services.AddInfrastructure();

        // Build service provider
        ServiceProvider = services.BuildServiceProvider();
        Mediator = ServiceProvider.GetRequiredService<IMediator>();

        // Ensure database is created
        await Context.Database.EnsureCreatedAsync();
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