using System.Net;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using VaultSharp.V1.SecretsEngines.Database;
using wg.modules.owner.application.Auth;
using wg.modules.owner.application.CQRS.Users.Commands.DeactivateUser;
using wg.modules.owner.application.CQRS.Users.Commands.SignIn;
using wg.modules.owner.application.CQRS.Users.Commands.SignUp;
using wg.modules.owner.application.CQRS.Users.Commands.VerifyUser;
using wg.modules.owner.application.CQRS.Users.Queries;
using wg.modules.owner.application.DTOs;
using wg.shared.abstractions.Auth.DTOs;
using wg.shared.abstractions.Context;
using wg.shared.abstractions.CQRS.Commands;
using wg.shared.abstractions.CQRS.Queries;
using wg.shared.infrastructure.Exceptions.DTOs;
using wg.shared.infrastructure.Pagination.Mappers;

namespace wg.modules.owner.api.Controllers;

internal sealed class UsersController(
    ICommandDispatcher commandDispatcher, 
    IQueryDispatcher queryDispatcher,
    ITokenStorage tokenStorage,
    IIdentityContext identityContext) : BaseController()
{
    [HttpGet]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(void), StatusCodes.Status401Unauthorized)]
    [SwaggerOperation("Gets all users by filters and pagination")]
    public async Task<ActionResult<List<UserDto>>> GetAll([FromQuery] GetUsersQuery query, CancellationToken cancellationToken)
    {
        var result = await queryDispatcher.SendAsync(query, cancellationToken);
        var metaData = result.AsMetaData();
        AddPaginationMetaData(metaData);
        return result.Any() ? Ok(result) : NoContent();
    }

    [HttpGet("group/{id:guid}")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(void), StatusCodes.Status401Unauthorized)]
    [SwaggerOperation(Summary = "Gets all users by group membership")]
    public async Task<ActionResult<UserDto>> GetForGroup([FromRoute] Guid id, CancellationToken cancellationToken)
        => Ok(await queryDispatcher.SendAsync(new GetUsersByGroupQuery(id), cancellationToken));

    [HttpGet("{id:guid}/active")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(void), StatusCodes.Status401Unauthorized)]
    [SwaggerOperation(Summary = "Gets user by \"ID\" only if user state is active")]
    public async Task<ActionResult<UserDto>> GetActiveUserById(Guid id, CancellationToken cancellationToken)
        => Ok(await queryDispatcher.SendAsync(new GetActiveUserByIdQuery(id), cancellationToken));

    [HttpGet("{id:guid}/is-active-exists")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(void), StatusCodes.Status401Unauthorized)]
    [SwaggerOperation("Gets existing of active user be \"ID\"")]
    public async Task<ActionResult<IsExistsDto>> IsActiveUserExists(Guid id, CancellationToken cancellationToken)
        => Ok(await queryDispatcher.SendAsync(new IsActiveUserExistsQuery(id), cancellationToken));
    
    [HttpGet("me")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(void), StatusCodes.Status401Unauthorized)]
    [SwaggerOperation(Summary = "Gets user by token in header")]
    public async Task<ActionResult<UserDto>> Me(CancellationToken cancellationToken)
    {
        var userId = identityContext.UserId;
        var result = await queryDispatcher.SendAsync(new GetUserByIdQuery(userId), cancellationToken);
        return Ok(result);
    }
    
    [HttpPost("sign-up")]
    [ProducesResponseType(typeof(void), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorDto), StatusCodes.Status400BadRequest)]
    [SwaggerOperation(Summary = "Registers user")]
    public async Task<ActionResult> SignUp(SignUpCommand command, CancellationToken cancellationToken)
    {
        await commandDispatcher.SendAsync(command with { Id = Guid.NewGuid() }, cancellationToken);
        return Ok();
    }

    [HttpPost("verify")]
    [ProducesResponseType(typeof(void), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorDto), StatusCodes.Status400BadRequest)]
    [SwaggerOperation(Summary = "Verifies user after register")]
    public async Task<ActionResult> Verify(VerifyUserCommand command, CancellationToken cancellationToken)
    {
        await commandDispatcher.SendAsync(command, cancellationToken);
        return Ok();
    }

    [HttpPost("sign-in")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorDto), StatusCodes.Status400BadRequest)]
    [SwaggerOperation(Summary = "Logs in user")]
    public async Task<ActionResult<JwtDto>> SignIn(SignInCommand command, CancellationToken cancellationToken)
    {
        await commandDispatcher.SendAsync(command, cancellationToken);
        var jwtToken = tokenStorage.Get();
        return Ok(jwtToken);
    }

    [HttpPatch("deactivate/{id:guid}")]
    [Authorize(Roles = "Manager")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorDto), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(void), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(void), StatusCodes.Status403Forbidden)]
    [SwaggerOperation("Deactivates user")]
    public async Task<ActionResult> Deactivate(Guid id, CancellationToken cancellationToken)
    {
        await commandDispatcher.SendAsync(new DeactivateUserCommand(id), cancellationToken);
        return Ok();
    }
}