using wg.modules.tickets.application.Clients.Companies.DTO;

namespace wg.modules.tickets.application.Clients.Companies;

public interface ICompaniesApiClient
{
    Task<IsProjectForCompanyDto> IsProjectForCompanyAsync(EmployeeWithProjectDto dto);
    Task<IsProjectActiveDto> IsProjectActiveAsync(ProjectIdDto dto);
    Task<IsActiveEmployeeExistsDto> IsActiveEmployeeExistsAsync(EmployeeIdDto dto);
}