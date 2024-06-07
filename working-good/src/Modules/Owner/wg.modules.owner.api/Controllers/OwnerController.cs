using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using wg.modules.owner.application.CQRS.Owners.Commands.AddOwner;
using wg.modules.owner.application.CQRS.Owners.Commands.ChangeOwnerName;
using wg.modules.owner.application.CQRS.Owners.Queries;
using wg.modules.owner.application.DTOs;
using wg.shared.abstractions.CQRS.Commands;
using wg.shared.abstractions.CQRS.Queries;
using wg.shared.infrastructure.Exceptions.DTOs;

namespace wg.modules.owner.api.Controllers;

internal sealed class OwnerController(
    ICommandDispatcher commandDispatcher, 
    IQueryDispatcher queryDispatcher) : BaseController()
{
    [HttpGet]
    [Authorize]
    [ProducesResponseType(typeof(OwnerDto),StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(void), StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(void), StatusCodes.Status401Unauthorized)]
    [SwaggerOperation("Gets owner")]
    public async Task<ActionResult<OwnerDto>> GetOwner([FromQuery]GetOwnerQuery query, CancellationToken cancellationToken)
        => Ok(await queryDispatcher.SendAsync(query, cancellationToken));
    
    [HttpPost("add")]
    [ProducesResponseType(typeof(void),StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ErrorDto), StatusCodes.Status400BadRequest)]
    [SwaggerOperation("Adds owner")]
    public async Task<ActionResult> AddOwner(AddOwnerCommand command, CancellationToken cancellationToken)
    {
        var id = Guid.NewGuid();
        await commandDispatcher.SendAsync(command with {Id = id}, cancellationToken);
        AddResourceHeader(id);
        return CreatedAtAction(nameof(GetOwner), null, null);
    }

    [HttpPatch("change-name")]
    [Authorize(Roles = "Manager")]
    [ProducesResponseType(typeof(void), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorDto), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(void), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(void), StatusCodes.Status403Forbidden)]
    [SwaggerOperation("Changes owner name")]
    public async Task<ActionResult> ChangeOwnerName(ChangeOwnerNameCommand command, CancellationToken cancellationToken)
    {
        await commandDispatcher.SendAsync(command, cancellationToken);
        return Ok();
    }
}