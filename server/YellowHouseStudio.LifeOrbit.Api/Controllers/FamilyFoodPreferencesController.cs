using MediatR;
using Microsoft.AspNetCore.Mvc;
using YellowHouseStudio.LifeOrbit.Application.Family.AddFoodPreference;
using YellowHouseStudio.LifeOrbit.Application.Family.RemoveFoodPreference;
using YellowHouseStudio.LifeOrbit.Application.Family.Common;

namespace YellowHouseStudio.LifeOrbit.Api.Controllers;

[ApiController]
[Route("settings/family/{familyMemberId}/preferences")]
public class FamilyFoodPreferencesController(IMediator mediator) : ControllerBase
{
    private readonly IMediator _mediator = mediator;

    /// <summary>
    /// Adds a food preference for a family member.
    /// </summary>
    /// <param name="familyMemberId">The family member's ID.</param>
    /// <param name="command">The add food preference command.</param>
    /// <returns>The updated family member response.</returns>
    /// <remarks>
    /// Valid values for <c>Status</c>: "Include", "AvailableForOthers", "NotAllowed".
    /// </remarks>
    /// <response code="200">Returns the updated family member</response>
    /// <response code="400">Validation error or bad request</response>
    /// <response code="404">Family member not found</response>
    [HttpPost]
    [ProducesResponseType(typeof(FamilyMemberResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<FamilyMemberResponse>> AddFoodPreference(Guid familyMemberId, [FromBody] AddFoodPreferenceCommand command)
    {
        if (familyMemberId != command.FamilyMemberId)
        {
            return BadRequest();
        }
        var result = await _mediator.Send(command);
        return Ok(result);
    }

    /// <summary>
    /// Removes a food preference from a family member.
    /// </summary>
    /// <param name="familyMemberId">The family member's ID.</param>
    /// <param name="foodItem">The food item to remove.</param>
    /// <returns>The updated family member response.</returns>
    /// <response code="200">Returns the updated family member</response>
    /// <response code="400">Validation error or bad request</response>
    /// <response code="404">Family member or food preference not found</response>
    [HttpDelete("{foodItem}")]
    [ProducesResponseType(typeof(FamilyMemberResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<FamilyMemberResponse>> RemoveFoodPreference(Guid familyMemberId, string foodItem)
    {
        var command = new RemoveFoodPreferenceCommand { FamilyMemberId = familyMemberId, FoodItem = foodItem };
        var result = await _mediator.Send(command);
        return Ok(result);
    }
}