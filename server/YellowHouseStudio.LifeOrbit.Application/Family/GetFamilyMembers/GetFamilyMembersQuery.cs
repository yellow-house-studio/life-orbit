using MediatR;
using YellowHouseStudio.LifeOrbit.Application.Family.Common;

namespace YellowHouseStudio.LifeOrbit.Application.Family.GetFamilyMembers;

public record GetFamilyMembersQuery() : IRequest<List<FamilyMemberResponse>>;