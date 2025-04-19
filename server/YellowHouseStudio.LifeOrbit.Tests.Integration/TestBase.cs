using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using YellowHouseStudio.LifeOrbit.Application.Data;

namespace YellowHouseStudio.LifeOrbit.Tests.Integration;

public abstract class TestBase
{
    protected ApplicationDbContext Context { get; private set; } = null!;

    [SetUp]
    public virtual void BaseSetup()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        Context = new ApplicationDbContext(options);
    }

    [TearDown]
    public virtual void BaseTearDown()
    {
        Context.Database.EnsureDeleted();
        Context.Dispose();
    }
} 