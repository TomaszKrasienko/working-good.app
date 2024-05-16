using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using wg.modules.owner.application.CQRS.Groups.Commands.AddUserToGroup;
using wg.modules.owner.application.CQRS.Groups.Queries;
using wg.shared.abstractions.CQRS.Commands;
using wg.shared.abstractions.CQRS.Queries;
using wg.shared.infrastructure.Exceptions.DTOs;

namespace wg.modules.owner.api.Controllers;

internal sealed class GroupsController(
    ICommandDispatcher commandDispatcher,
    IQueryDispatcher queryDispatcher) : BaseController
{
    [HttpGet("{groupId:guid}/{userId:guid}/is-membership-exists")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult> IsMembershipExists(Guid groupId, Guid userId, CancellationToken cancellationToken)
        => Ok(await queryDispatcher.SendAsync(new IsMembershipExistsQuery(userId, groupId), cancellationToken));
    
    [HttpPost("{groupId:guid}/add-user")]
    [Authorize(Roles = "Manager")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ErrorDto), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(void), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(void), StatusCodes.Status403Forbidden)]
    [SwaggerOperation("Adds user to group")]
    public async Task<ActionResult> AddUserToGroup(Guid groupId, AddUserToGroupCommand command, CancellationToken cancellationToken)
    {
        await commandDispatcher.SendAsync(command with { GroupId = groupId }, cancellationToken);
        return NoContent();
    }
    
    
}