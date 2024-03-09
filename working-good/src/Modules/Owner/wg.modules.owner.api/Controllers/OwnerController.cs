using System.Net;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using wg.modules.owner.application.CQRS.Owners.Commands.AddOwner;
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
}