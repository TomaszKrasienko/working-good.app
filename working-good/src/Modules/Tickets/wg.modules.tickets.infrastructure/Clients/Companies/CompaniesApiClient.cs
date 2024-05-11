using wg.modules.tickets.application.Clients.Companies;
using wg.modules.tickets.application.Clients.Companies.DTO;
using wg.shared.abstractions.Modules;

namespace wg.modules.tickets.infrastructure.Clients.Companies;

internal sealed class CompaniesApiClient(
    IModuleClient moduleClient) : ICompaniesApiClient
{
    public Task<CompanyDto> GetCompanyByEmployeeIdAsync(EmployeeIdDto dto)
        => moduleClient.SendAsync<CompanyDto>("companies/get/by-employee-id", dto);

    public Task<IsProjectForCompanyDto> IsProjectForCompanyAsync(EmployeeWithProjectDto dto)
        => moduleClient.SendAsync<IsProjectForCompanyDto>("companies/is-project-for-company/get", dto);

    public Task<IsProjectActiveDto> IsProjectActiveAsync(ProjectIdDto dto)
        => moduleClient.SendAsync<IsProjectActiveDto>("companies/is-project-active/get", dto);
}