using Microsoft.AspNetCore.Mvc;
using wg.modules.owner.application.CQRS.Owners.Commands.AddOwner;
using wg.shared.abstractions.CQRS.Commands;
using wg.shared.abstractions.CQRS.Queries;

namespace wg.modules.owner.api.Controllers;

internal sealed class OwnersController(
    ICommandDispatcher commandDispatcher, 
    IQueryDispatcher queryDispatcher) : BaseController()
{

    [HttpPost("add")]
    public async Task<ActionResult> AddOwner(AddOwnerCommand command, CancellationToken cancellationToken)
    {
        var id = Guid.NewGuid();
        await commandDispatcher.SendAsync(command with {Id = id}, cancellationToken);
        AddResourceHeader(id);
        return Created();
    }
}