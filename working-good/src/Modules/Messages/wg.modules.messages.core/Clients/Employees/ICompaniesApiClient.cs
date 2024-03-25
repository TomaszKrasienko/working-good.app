using wg.modules.messages.core.Clients.Employees.DTO;

namespace wg.modules.messages.core.Clients.Employees;

public interface ICompaniesApiClient
{
    Task<IsEmployeeExistsDto> IsEmployeeExists(EmployeeEmailDto dto);
}