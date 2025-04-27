using MediatR;
using Microsoft.AspNetCore.Mvc;
using YellowHouseStudio.LifeOrbit.Application.Family.AddFamilyMember;
using YellowHouseStudio.LifeOrbit.Application.Family.GetFamilyMembers;
using YellowHouseStudio.LifeOrbit.Application.Family.AddCompleteFamilyMember;
using YellowHouseStudio.LifeOrbit.Application.Family.Common;
using YellowHouseStudio.LifeOrbit.Application.Family.UpdateAllergy;
using YellowHouseStudio.LifeOrbit.Application.Family.UpdateFoodPreference;
using YellowHouseStudio.LifeOrbit.Application.Family.RemoveFoodPreference;

namespace YellowHouseStudio.LifeOrbit.Api.Controllers;

[ApiController]
[Route("settings/[controller]")]
public class FamilyController(IMediator mediator) : ControllerBase
{
    private readonly IMediator _mediator = mediator;

    [HttpGet]
    public async Task<ActionResult<List<FamilyMemberResponse>>> GetFamilyMembers()
    {
        var query = new GetFamilyMembersQuery();
        var result = await _mediator.Send(query);
        return Ok(result);
    }

    [HttpPost]
    public async Task<ActionResult<Guid>> AddFamilyMember(AddFamilyMemberCommand command)
    {
        var result = await _mediator.Send(command);
        return Ok(result);
    }

    [HttpPost("complete")]
    public async Task<ActionResult<FamilyMemberResponse>> AddCompleteFamilyMember(AddCompleteFamilyMemberCommand command)
    {
        var result = await _mediator.Send(command);
        return Created($"settings/family/{result.Id}", result);
    }
}