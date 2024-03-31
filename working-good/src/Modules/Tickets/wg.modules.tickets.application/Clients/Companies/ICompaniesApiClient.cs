using wg.modules.tickets.application.Clients.Companies.DTO;

namespace wg.modules.tickets.application.Clients.Companies;

public interface ICompaniesApiClient
{
    Task<CompanySlaTimeDto> GetSlaTimeByEmployeeAsync(EmployeeIdDto dto);
    Task<IsEmployeeExistsDto> IsEmployeeExistsAsync(EmployeeIdDto dto);
    Task<IsProjectExistsDto> IsProjectExistsAsync(EmployeeWithProjectDto dto);
}