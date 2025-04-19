using MediatR;
using YellowHouseStudio.LifeOrbit.Application.Data;
using YellowHouseStudio.LifeOrbit.Domain.Family;

namespace YellowHouseStudio.LifeOrbit.Application.Family.AddFamilyMember;

public class AddFamilyMemberCommandHandler : IRequestHandler<AddFamilyMemberCommand, Guid>
{
    private readonly ApplicationDbContext _context;

    public AddFamilyMemberCommandHandler(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Guid> Handle(AddFamilyMemberCommand request, CancellationToken cancellationToken)
    {
        var familyMember = new FamilyMember(request.UserId, request.Name, request.Age);

        _context.FamilyMembers.Add(familyMember);
        await _context.SaveChangesAsync(cancellationToken);

        return familyMember.Id;
    }
} 