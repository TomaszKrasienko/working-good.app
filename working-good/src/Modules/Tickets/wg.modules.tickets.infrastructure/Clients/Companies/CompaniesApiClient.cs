using wg.modules.tickets.application.Clients.Companies;
using wg.modules.tickets.application.Clients.Companies.DTO;
using wg.shared.abstractions.Modules;

namespace wg.modules.tickets.infrastructure.Clients.Companies;

internal sealed class CompaniesApiClient(
    IModuleClient moduleClient) : ICompaniesApiClient
{
    public async Task<CompanySlaTimeDto> GetSlaTimeByEmployee(EmployeeIdDto dto)
        => await moduleClient.SendAsync<CompanySlaTimeDto>("companies/employee/sla-time/get", dto);

    public async Task<IsEmployeeExistsDto> IsEmployeeExists(EmployeeIdDto dto)
        => await moduleClient.SendAsync<IsEmployeeExistsDto>("companies/employee/is-exists/get", dto);

    public async Task<IsProjectExistsDto> IsProjectExists(EmployeeWithProjectDto dto)
        => await moduleClient.SendAsync<IsProjectExistsDto>("companies/project/is-exists/get", dto);
}