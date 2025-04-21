using MediatR;
using YellowHouseStudio.LifeOrbit.Application.Family.Common;

namespace YellowHouseStudio.LifeOrbit.Application.Family.GetFamilyMembers;

public record GetFamilyMembersQuery(Guid UserId) : IRequest<List<FamilyMemberResponse>>;