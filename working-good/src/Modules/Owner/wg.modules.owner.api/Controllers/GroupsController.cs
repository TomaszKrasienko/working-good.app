using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using wg.modules.owner.application.CQRS.Groups.Commands.AddUserToGroup;
using wg.modules.owner.application.DTOs;
using wg.shared.abstractions.CQRS.Commands;

namespace wg.modules.owner.api.Controllers;

internal sealed class GroupsController(
    ICommandDispatcher commandDispatcher) : BaseController 
{
    [HttpPost("{groupId:guid}/add-user")]
    [Authorize(Roles = "Manager")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<ActionResult> AddUserToGroup(Guid groupId, AddUserToGroupCommand command, CancellationToken cancellationToken)
    {
        await commandDispatcher.SendAsync(command with { GroupId = groupId }, cancellationToken);
        return NoContent();
    }
}