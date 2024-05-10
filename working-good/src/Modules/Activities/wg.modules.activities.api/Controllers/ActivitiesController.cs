using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using wg.modules.activities.application.CQRS.Activities.Commands.AddActivity;
using wg.modules.activities.application.CQRS.Activities.Queries;
using wg.modules.activities.application.DTOs;
using wg.shared.abstractions.CQRS.Commands;
using wg.shared.abstractions.CQRS.Queries;

namespace wg.modules.activities.api.Controllers;

internal sealed class ActivitiesController(
    ICommandDispatcher commandDispatcher,
    IQueryDispatcher queryDispatcher) : BaseController
{
    [HttpGet("{id:guid}")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [SwaggerOperation(Summary = "Gets activity by identifier")]
    public async Task<ActionResult<ActivityDto>> GetById(Guid id, CancellationToken cancellationToken)
        => Ok(await queryDispatcher.SendAsync(new GetActivityById(id), cancellationToken));
    
    [HttpPost("add")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [SwaggerOperation(Summary = "Adding new activity", Description = @"Endpoint for adding new activity
    to ticket for user")]
    public async Task<ActionResult> AddActivity(AddActivityCommand command, CancellationToken cancellationToken)
    {
        var activityId = Guid.NewGuid();
        await commandDispatcher.SendAsync(command with { Id = activityId }, cancellationToken);
        AddResourceHeader(activityId);
        return CreatedAtAction(nameof(GetById), new {id = activityId}, null);
    }
}