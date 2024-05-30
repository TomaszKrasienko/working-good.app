using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using wg.modules.companies.application.CQRS.Employees.Queries;
using wg.modules.companies.application.CQRS.Projects.Commands.AddProject;
using wg.modules.companies.application.CQRS.Projects.Commands.EditProject;
using wg.modules.companies.application.CQRS.Projects.Queries;
using wg.modules.companies.application.DTOs;
using wg.shared.abstractions.CQRS.Commands;
using wg.shared.abstractions.CQRS.Queries;
using wg.shared.infrastructure.Exceptions.DTOs;

namespace wg.modules.companies.api.Controllers;

internal sealed class ProjectsController(
    ICommandDispatcher commandDispatcher,
    IQueryDispatcher queryDispatcher) : BaseController
{
    [HttpGet("{projectId:guid}")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(void), StatusCodes.Status401Unauthorized)]
    [SwaggerOperation("Gets project by \"ID\"")]
    public async Task<ActionResult<ProjectDto>> GetById(Guid projectId, CancellationToken cancellationToken)
        => await queryDispatcher.SendAsync(new GetProjectByIdQuery(projectId), cancellationToken);

    [HttpGet("{projectId:guid}/active")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(void), StatusCodes.Status401Unauthorized)]
    [SwaggerOperation("Gets whether there is an active project")]
    public async Task<ActionResult<IsExistsDto>> IsProjectActive(Guid projectId, CancellationToken cancellationToken)
        => Ok(await queryDispatcher.SendAsync(new IsProjectActiveQuery(projectId), cancellationToken));

    [HttpGet("{projectId:guid}/employee/{employeeId:guid}/active")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(void),StatusCodes.Status401Unauthorized)]
    [SwaggerOperation("Gets whether there is an active project for company with employee")]
    public async Task<ActionResult<IsExistsDto>> IsActiveProjectForEmployeeExists(Guid projectId, Guid employeeId,
        CancellationToken cancellationToken)
        => Ok(await queryDispatcher.SendAsync(new IsProjectInCompanyQuery(employeeId, projectId), cancellationToken));
    
    [HttpPost("companies/{companyId:guid}/add")]
    [Authorize(Roles = "Manager")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ErrorDto), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(void), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(void), StatusCodes.Status403Forbidden)]
    [SwaggerOperation("Adds project")]
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
    [ProducesResponseType(typeof(void), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorDto), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(void), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(void), StatusCodes.Status403Forbidden)]
    [SwaggerOperation("Edits project")]
    public async Task<ActionResult> EditProject(EditProjectCommand command, Guid id, CancellationToken cancellationToken)
    {
        await commandDispatcher.SendAsync(command with { Id = id }, cancellationToken);
        return Ok();
    }
}