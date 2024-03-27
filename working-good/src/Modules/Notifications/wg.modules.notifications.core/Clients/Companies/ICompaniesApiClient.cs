using wg.modules.notifications.core.Clients.Companies.DTO;

namespace wg.modules.notifications.core.Clients.Companies;

public interface ICompaniesApiClient
{
    Task<EmployeeEmailDto> GetEmployeeEmail(EmployeeIdDto dto);
}