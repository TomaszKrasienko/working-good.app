using wg.modules.messages.core.Clients.Employees.DTO;

namespace wg.modules.messages.core.Clients.Employees;

public interface ICompaniesApiClient
{
    Task<EmployeeIdDto> GetEmployeeId(EmployeeEmailDto dto);
}