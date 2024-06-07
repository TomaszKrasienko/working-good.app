using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using wg.modules.companies.application.CQRS.Companies.Commands.AddCompany;
using wg.modules.companies.application.CQRS.Companies.Commands.UpdateCompany;
using wg.modules.companies.application.CQRS.Companies.Queries;
using wg.modules.companies.application.DTOs;
using wg.modules.companies.domain.ValueObjects.Company;
using wg.shared.abstractions.CQRS.Commands;
using wg.shared.abstractions.CQRS.Queries;
using wg.shared.infrastructure.Exceptions.DTOs;
using wg.shared.infrastructure.Pagination.Mappers;

namespace wg.modules.companies.api.Controllers;

[Authorize]
internal sealed class CompaniesController(
    ICommandDispatcher commandDispatcher, 
    IQueryDispatcher queryDispatcher) : BaseController
{
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<CompanyDto>),StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(void),StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(void), StatusCodes.Status401Unauthorized)]
    [SwaggerOperation("Gets all companies by filters and pagination")]
    public async Task<ActionResult<IEnumerable<CompanyDto>>> GetAll([FromQuery] GetCompaniesQuery query, CancellationToken cancellationToken)
    {
        var result = await queryDispatcher.SendAsync(query, cancellationToken);
        var metaData = result.AsMetaData();
        AddPaginationMetaData(metaData);
        return Ok(result);
    }
    
    [HttpGet("{companyId:guid}")]
    [ProducesResponseType(typeof(CompanyDto),StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(void), StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(void), StatusCodes.Status401Unauthorized)]
    [SwaggerOperation("Gets company by \"ID\"")]
    public async Task<ActionResult<CompanyDto>> GetById(Guid companyId, CancellationToken cancellationToken)
        => await queryDispatcher.SendAsync(new GetCompanyByIdQuery(companyId), cancellationToken);

    [HttpGet("{companyId:guid}/is-active")]
    [ProducesResponseType(typeof(IsExistsDto),StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(void), StatusCodes.Status401Unauthorized)]
    [SwaggerOperation("Gets existing of active company")]
    public async Task<ActionResult<IsExistsDto>> IsActive(Guid companyId, CancellationToken cancellationToken)
        => await queryDispatcher.SendAsync(new IsActiveCompanyExistsQuery(companyId), cancellationToken);

    [HttpGet("sla-time/{employeeId:guid}")]
    [ProducesResponseType(typeof(SlaTimeDto),StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(void), StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(void), StatusCodes.Status401Unauthorized)]
    [SwaggerOperation("Gets sla time by \"EmployeeId\"")]
    public async Task<ActionResult<SlaTimeDto>> GetSlaTimeByEmployeeId(Guid employeeId, CancellationToken cancellationToken)
        => Ok(await queryDispatcher.SendAsync(new GetSlaTimeByEmployeeIdQuery(employeeId), cancellationToken));
    
    [Authorize(Roles = "Manager")]
    [HttpPost("add")]
    [ProducesResponseType(typeof(void),StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ErrorDto), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(void), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(void), StatusCodes.Status403Forbidden)]
    [SwaggerOperation("Adds company")]
    public async Task<ActionResult> AddCompany(AddCompanyCommand command, CancellationToken cancellationToken)
    {
        var companyId = Guid.NewGuid();
        await commandDispatcher.SendAsync(command with { Id = companyId }, cancellationToken);
        AddResourceHeader(companyId);
        return CreatedAtAction(nameof(GetById), new { id = companyId }, null);
    }

    [Authorize(Roles = "Manager")]
    [HttpPut("{companyId:guid}")]
    [ProducesResponseType(typeof(void),StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorDto), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(void), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(void), StatusCodes.Status403Forbidden)]
    [SwaggerOperation("Updates company")]
    public async Task<ActionResult> UpdateCompany(Guid companyId, UpdateCompanyCommand command, CancellationToken cancellationToken)
    {
        await commandDispatcher.SendAsync(command with { Id = companyId }, cancellationToken);
        return Ok();
    }
}