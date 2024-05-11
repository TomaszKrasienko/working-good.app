using wg.modules.tickets.application.Clients.Companies.DTO;

namespace wg.modules.tickets.application.Clients.Companies;

public interface ICompaniesApiClient
{
    Task<CompanyDto> GetCompanyByEmployeeIdAsync(EmployeeIdDto dto);
    Task<IsProjectForCompanyDto> IsProjectForCompanyAsync(EmployeeWithProjectDto dto);
    Task<IsProjectActiveDto> IsProjectActiveAsync(ProjectIdDto dto);
}