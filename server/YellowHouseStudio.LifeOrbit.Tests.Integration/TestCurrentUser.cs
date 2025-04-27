using YellowHouseStudio.LifeOrbit.Application.Users;

namespace YellowHouseStudio.LifeOrbit.Tests.Integration;

public class TestCurrentUser : ICurrentUser
{
    public TestCurrentUser()
    {
        UserId = Guid.NewGuid();
    }

    public Guid UserId { get; }
}