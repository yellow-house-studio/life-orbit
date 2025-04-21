using YellowHouseStudio.LifeOrbit.Application.Users;

namespace YellowHouseStudio.LifeOrbit.Api.Services;

public class FakeCurrentUser : ICurrentUser
{
    public Guid UserId { get; } = new Guid("123e4567-e89b-12d3-a456-426614174000");
}
