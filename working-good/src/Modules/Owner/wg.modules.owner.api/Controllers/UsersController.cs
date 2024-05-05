using System.Net;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using wg.modules.owner.application.Auth;
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
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [SwaggerOperation(Summary = "Gets all users by filters and pagination")]
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
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [SwaggerOperation(Summary = "Gets all users by group membership")]
    public async Task<ActionResult<UserDto>> GetForGroup([FromRoute] Guid id, CancellationToken cancellationToken)
        => Ok(await queryDispatcher.SendAsync(new GetUsersByGroupQuery(id), cancellationToken));

    [HttpGet("{id:guid}/active")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [SwaggerOperation(Summary = "Gets user by id only if user state is active")]
    public async Task<ActionResult<UserDto>> GetActiveUserById(Guid id, CancellationToken cancellationToken)
        => Ok(await queryDispatcher.SendAsync(new GetActiveUserByIdQuery(id), cancellationToken));
    
    [HttpGet("me")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [SwaggerOperation(Summary = "Gets user by token in header")]
    public async Task<ActionResult<UserDto>> Me(CancellationToken cancellationToken)
    {
        var userId = identityContext.UserId;
        var result = await queryDispatcher.SendAsync(new GetUserByIdQuery(userId), cancellationToken);
        return Ok(result);
    }
    
    [HttpPost("sign-up")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ErrorDto))]
    [SwaggerOperation(Summary = "Registers user")]
    public async Task<ActionResult> SignUp(SignUpCommand command, CancellationToken cancellationToken)
    {
        await commandDispatcher.SendAsync(command with { Id = Guid.NewGuid() }, cancellationToken);
        return Ok();
    }

    [HttpPost("verify")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [SwaggerOperation(Summary = "Verifies user after register")]
    public async Task<ActionResult> Verify(VerifyUserCommand command, CancellationToken cancellationToken)
    {
        await commandDispatcher.SendAsync(command, cancellationToken);
        return Ok();
    }

    [HttpPost("sign-in")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [SwaggerOperation(Summary = "Logs in user")]
    public async Task<ActionResult<JwtDto>> SignIn(SignInCommand command, CancellationToken cancellationToken)
    {
        await commandDispatcher.SendAsync(command, cancellationToken);
        var jwtToken = tokenStorage.Get();
        return Ok(jwtToken);
    }


    
}