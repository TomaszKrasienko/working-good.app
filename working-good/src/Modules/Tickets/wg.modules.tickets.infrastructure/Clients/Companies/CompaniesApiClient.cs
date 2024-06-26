using wg.modules.tickets.application.Clients.Companies;
using wg.modules.tickets.application.Clients.Companies.DTO;
using wg.shared.abstractions.Modules;

namespace wg.modules.tickets.infrastructure.Clients.Companies;

internal sealed class CompaniesApiClient(
    IModuleClient moduleClient) : ICompaniesApiClient
{
    public Task<IsProjectForCompanyDto> IsProjectForCompanyAsync(EmployeeWithProjectDto dto)
        => moduleClient.SendAsync<IsProjectForCompanyDto>("companies/projects/is-project-for-company/get", dto);

    public Task<IsProjectActiveDto> IsProjectActiveAsync(ProjectIdDto dto)
        => moduleClient.SendAsync<IsProjectActiveDto>("companies/projects/is-active-exists/get", dto);

    public Task<IsActiveEmployeeExistsDto> IsActiveEmployeeExistsAsync(EmployeeIdDto dto)
        => moduleClient.SendAsync<IsActiveEmployeeExistsDto>("companies/employees/is-active-exists/get", dto);

    public Task<SlaTimeDto> GetSlaTimeByEmployeeAsync(EmployeeIdDto dto)
        => moduleClient.SendAsync<SlaTimeDto>("companies/sla-time/by-employee/get", dto);
}