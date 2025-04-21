using YellowHouseStudio.LifeOrbit.Application.Common.Commands;

namespace YellowHouseStudio.LifeOrbit.Application.Family.AddFamilyMember;

public record AddFamilyMemberCommand : ICommand<Guid>
{
    public Guid UserId { get; init; }
    public string Name { get; init; } = null!;
    public int Age { get; init; }
}