using wg.modules.tickets.application.Clients.Companies.DTO;

namespace wg.modules.tickets.application.Clients.Companies;

public interface ICompaniesApiClient
{
    Task<CompanySlaTimeDto> GetSlaTimeByEmployee(EmployeeIdDto dto);
    Task<IsEmployeeExistsDto> IsEmployeeExists(EmployeeIdDto dto);
    Task<IsProjectExistsDto> IsProjectExists(EmployeeWithProjectDto dto);
}