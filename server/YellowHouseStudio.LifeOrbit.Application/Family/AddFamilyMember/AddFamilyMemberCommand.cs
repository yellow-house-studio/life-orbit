using MediatR;

namespace YellowHouseStudio.LifeOrbit.Application.Family.AddFamilyMember;

public record AddFamilyMemberCommand : IRequest<Guid>
{
    public Guid UserId { get; init; }
    public string Name { get; init; } = null!;
    public int Age { get; init; }
}