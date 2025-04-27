namespace YellowHouseStudio.LifeOrbit.Application.Family.Common;

public record SafeFoodResult
{
    public Guid FamilyMemberId { get; init; }
    public List<string> SafeFoods { get; init; } = new();
}