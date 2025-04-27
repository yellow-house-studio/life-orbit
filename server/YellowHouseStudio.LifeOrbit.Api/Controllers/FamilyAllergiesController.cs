using MediatR;
using Microsoft.AspNetCore.Mvc;
using YellowHouseStudio.LifeOrbit.Application.Family.AddAllergy;
using YellowHouseStudio.LifeOrbit.Application.Family.RemoveAllergy;
using YellowHouseStudio.LifeOrbit.Application.Family.Common;

namespace YellowHouseStudio.LifeOrbit.Api.Controllers;

[ApiController]
[Route("settings/family/{familyMemberId}/allergies")]
public class FamilyAllergiesController(IMediator mediator) : ControllerBase
{
    private readonly IMediator _mediator = mediator;

    /// <summary>
    /// Adds an allergy to a family member
    /// </summary>
    /// <param name="familyMemberId">The ID of the family member</param>
    /// <param name="command">The allergy details</param>
    /// <returns>The updated family member</returns>
    [HttpPost]
    [ProducesResponseType(typeof(FamilyMemberResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<FamilyMemberResponse>> AddAllergy(Guid familyMemberId, AddAllergyCommand command)
    {
        if (familyMemberId != command.FamilyMemberId)
        {
            return BadRequest("Family member ID in route must match ID in command");
        }
        var result = await _mediator.Send(command);
        return Ok(result);
    }

    /// <summary>
    /// Removes an allergy from a family member
    /// </summary>
    /// <param name="familyMemberId">The ID of the family member</param>
    /// <param name="allergen">The allergen to remove</param>
    /// <returns>The updated family member</returns>
    [HttpDelete("{allergen}")]
    [ProducesResponseType(typeof(FamilyMemberResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<FamilyMemberResponse>> RemoveAllergy(Guid familyMemberId, string allergen)
    {
        var command = new RemoveAllergyCommand { FamilyMemberId = familyMemberId, Allergen = allergen };
        var result = await _mediator.Send(command);
        return Ok(result);
    }
} 