using System.Net;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using wg.modules.owner.application.CQRS.Owners.Commands.AddOwner;
using wg.modules.owner.application.CQRS.Owners.Commands.ChangeOwnerName;
using wg.shared.abstractions.CQRS.Commands;
using wg.shared.abstractions.CQRS.Queries;

namespace wg.modules.owner.api.Controllers;

internal sealed class OwnerController(
    ICommandDispatcher commandDispatcher, 
    IQueryDispatcher queryDispatcher) : BaseController()
{

    [HttpPost("add")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> AddOwner(AddOwnerCommand command, CancellationToken cancellationToken)
    {
        var id = Guid.NewGuid();
        await commandDispatcher.SendAsync(command with {Id = id}, cancellationToken);
        AddResourceHeader(id);
        return NoContent();
    }

    [HttpPatch("change-name")]
    [Authorize(Roles = "Manager")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<ActionResult> ChangeOwnerName(ChangeOwnerNameCommand command, CancellationToken cancellationToken)
    {
        await commandDispatcher.SendAsync(command, cancellationToken);
        return NoContent();
    }
}