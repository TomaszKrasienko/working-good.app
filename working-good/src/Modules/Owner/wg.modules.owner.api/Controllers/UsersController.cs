using System.Net;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using wg.modules.owner.application.CQRS.Users.Commands.SignUp;
using wg.shared.abstractions.CQRS.Commands;
using wg.shared.abstractions.CQRS.Queries;

namespace wg.modules.owner.api.Controllers;

internal sealed class UsersController(
    ICommandDispatcher commandDispatcher, 
    IQueryDispatcher queryDispatcher) : BaseController()
{
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [HttpPost("sign-up")]
    public async Task<ActionResult> SignUp(SignUpCommand command, CancellationToken cancellationToken)
    {
        await commandDispatcher.SendAsync(command with { Id = Guid.NewGuid() }, cancellationToken);
        return Ok();
    }
}