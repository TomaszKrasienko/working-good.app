using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using wg.modules.companies.application.CQRS.Employees.Commands.AddEmployee;
using wg.modules.companies.application.CQRS.Employees.Commands.DeactivateEmployee;
using wg.modules.companies.application.CQRS.Employees.Queries;
using wg.modules.companies.application.DTOs;
using wg.shared.abstractions.CQRS.Commands;
using wg.shared.abstractions.CQRS.Queries;

namespace wg.modules.companies.api.Controllers;

internal sealed class EmployeesController(
    ICommandDispatcher commandDispatcher, 
    IQueryDispatcher queryDispatcher) : BaseController
{
    [HttpGet("{id:guid}")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<EmployeeDto>> GetById(Guid id, CancellationToken cancellationToken)
        => Ok(await queryDispatcher.SendAsync(new GetEmployeeByIdQuery(id), cancellationToken));

    [HttpGet("{id:guid}/active")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(void), StatusCodes.Status401Unauthorized)]
    [SwaggerOperation("Gets active employee by \"ID\"")]
    public async Task<ActionResult<EmployeeDto>> GetActiveById(Guid id, CancellationToken cancellationToken)
        => await queryDispatcher.SendAsync(new GetActiveEmployeeByIdQuery(id), cancellationToken);


    [HttpGet("{id:guid}/is-active")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<IsExistsDto>> IsEmployeeActiveById(Guid id, CancellationToken cancellationToken)
        => await queryDispatcher.SendAsync(new IsActiveEmployeeExistsQuery(id), cancellationToken);
    
    [HttpPost("companies/{companyId}/add")]
    [Authorize(Roles = "Manager")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> AddEmployee(Guid companyId, AddEmployeeCommand command, CancellationToken cancellationToken)
    {
        var employeeId = Guid.NewGuid();
        await commandDispatcher.SendAsync(command with { CompanyId = companyId, Id = employeeId }, cancellationToken);
        AddResourceHeader(employeeId);
        return CreatedAtAction(nameof(GetById), new { id = companyId }, null);
    }

    [HttpPatch("deactivate/{id:guid}")]
    [Authorize(Roles = "Manager")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<ActionResult> DeactivateEmployee(Guid id, DeactivateEmployeeCommand command, CancellationToken cancellationToken)
    {
        await commandDispatcher.SendAsync(command with { Id = id }, cancellationToken);
        return Ok();
    }
}