using System.Net;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using wg.modules.owner.application.Auth;
using wg.modules.owner.application.CQRS.Users.Commands.SignIn;
using wg.modules.owner.application.CQRS.Users.Commands.SignUp;
using wg.modules.owner.application.CQRS.Users.Commands.VerifyUser;
using wg.shared.abstractions.Auth.DTOs;
using wg.shared.abstractions.CQRS.Commands;
using wg.shared.abstractions.CQRS.Queries;

namespace wg.modules.owner.api.Controllers;

internal sealed class UsersController(
    ICommandDispatcher commandDispatcher, 
    IQueryDispatcher queryDispatcher,
    ITokenStorage tokenStorage) : BaseController()
{
    [HttpPost("sign-up")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> SignUp(SignUpCommand command, CancellationToken cancellationToken)
    {
        await commandDispatcher.SendAsync(command with { Id = Guid.NewGuid() }, cancellationToken);
        return Ok();
    }

    [HttpPost("verify")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> Verify(VerifyUserCommand command, CancellationToken cancellationToken)
    {
        await commandDispatcher.SendAsync(command, cancellationToken);
        return Ok();
    }

    [HttpPost("sign-in")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<JwtDto>> SignIn(SignInCommand command, CancellationToken cancellationToken)
    {
        await commandDispatcher.SendAsync(command, cancellationToken);
        var jwtToken = tokenStorage.Get();
        return Ok(jwtToken);
    }
}