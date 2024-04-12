using wg.modules.tickets.application.Clients.Companies;
using wg.modules.tickets.application.Clients.Companies.DTO;
using wg.shared.abstractions.Modules;

namespace wg.modules.tickets.infrastructure.Clients.Companies;

internal sealed class CompaniesApiClient(
    IModuleClient moduleClient) : ICompaniesApiClient
{
    public Task<CompanyDto> GetCompanyByEmployeeIdAsync(EmployeeIdDto dto)
        => moduleClient.SendAsync<CompanyDto>("companies/get/by-employee-id", dto);
    public Task<IsEmployeeExistsDto> IsEmployeeExistsAsync(EmployeeIdDto dto)
        => moduleClient.SendAsync<IsEmployeeExistsDto>("companies/employee/is-exists/get", dto);
    public Task<IsProjectExistsDto> IsProjectExistsAsync(EmployeeWithProjectDto dto)
        => moduleClient.SendAsync<IsProjectExistsDto>("companies/project/is-exists/get", dto);
}