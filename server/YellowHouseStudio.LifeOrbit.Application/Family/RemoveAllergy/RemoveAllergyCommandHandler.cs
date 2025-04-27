using MediatR;
using Microsoft.Extensions.Logging;
using YellowHouseStudio.LifeOrbit.Application.Family.Common;
using YellowHouseStudio.LifeOrbit.Application.Users;

namespace YellowHouseStudio.LifeOrbit.Application.Family.RemoveAllergy;

public class RemoveAllergyCommandHandler : IRequestHandler<RemoveAllergyCommand, FamilyMemberResponse>
{
    private readonly IFamilyMemberRepository _repository;
    private readonly ICurrentUser _currentUser;
    private readonly ILogger<RemoveAllergyCommandHandler> _logger;

    public RemoveAllergyCommandHandler(
        IFamilyMemberRepository repository,
        ICurrentUser currentUser,
        ILogger<RemoveAllergyCommandHandler> logger)
    {
        _repository = repository;
        _currentUser = currentUser;
        _logger = logger;
    }

    public async Task<FamilyMemberResponse> Handle(RemoveAllergyCommand request, CancellationToken cancellationToken)
    {
        _logger.LogDebug("Removing allergy {Allergen} for family member {FamilyMemberId}", request.Allergen, request.FamilyMemberId);

        var familyMember = await _repository.GetByIdWithAllergiesAsync(request.FamilyMemberId, _currentUser.UserId, cancellationToken);
        // Existence and allergy presence are validated in the validator
        familyMember!.RemoveAllergy(request.Allergen);
        _logger.LogInformation("Successfully removed allergy {Allergen} for family member {FamilyMemberId}", request.Allergen, request.FamilyMemberId);
        return FamilyMemberResponse.FromDomain(familyMember);
    }
}