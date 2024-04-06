using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using wg.modules.companies.application.CQRS.Projects.Commands.AddProject;
using wg.modules.companies.application.CQRS.Projects.Commands.EditProject;
using wg.shared.abstractions.CQRS.Commands;

namespace wg.modules.companies.api.Controllers;

internal sealed class ProjectsController(
    ICommandDispatcher commandDispatcher) : BaseController
{
    [HttpGet("{id:guid}")]
    [Authorize]
    public async Task<ActionResult> GetById(Guid id)
    {
        throw new NotImplementedException();
    }
    
    [HttpPost("companies/{companyId:guid}/add")]
    [Authorize(Roles = "Manager")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> AddProject(AddProjectCommand command, Guid companyId, CancellationToken cancellationToken)
    {
        var projectId = Guid.NewGuid();
        await commandDispatcher.SendAsync(command with
        {
            CompanyId = companyId,
            Id = projectId
        }, cancellationToken);
        AddResourceHeader(projectId);
        return CreatedAtAction(nameof(GetById), new { id = companyId }, null);
    }

    [HttpPut("edit/{id:guid}")]
    [Authorize(Roles = "Manager")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<ActionResult> EditProject(EditProjectCommand command, Guid id, CancellationToken cancellationToken)
    {
        await commandDispatcher.SendAsync(command with { Id = id }, cancellationToken);
        return Ok();
    }
}