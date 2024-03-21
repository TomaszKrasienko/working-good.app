using wg.modules.tickets.application.Clients.Companies;
using wg.modules.tickets.application.Clients.Companies.DTO;
using wg.shared.abstractions.Modules;

namespace wg.modules.tickets.infrastructure.Clients.Companies;

internal sealed class CompaniesApiClient(
    IModuleClient moduleClient) : ICompaniesApiClient
{
    public Task<CompanySlaTimeDto> GetSlaTimeByEmployee(EmployeeIdDto dto)
        => moduleClient.SendAsync<CompanySlaTimeDto>("companies/employee/sla-time/get", dto);

    public Task<IsEmployeeExistsDto> IsEmployeeExists(EmployeeIdDto dto)
        => moduleClient.SendAsync<IsEmployeeExistsDto>("companies/employee/is-exists/get", dto);

    public Task<IsProjectExistsDto> IsProjectExists(EmployeeWithProjectDto dto)
        => moduleClient.SendAsync<IsProjectExistsDto>("companies/project/is-exists/get", dto);
}