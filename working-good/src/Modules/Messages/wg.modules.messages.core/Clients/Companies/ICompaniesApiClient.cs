using wg.modules.messages.core.Clients.Companies.DTO;

namespace wg.modules.messages.core.Clients.Companies;

public interface ICompaniesApiClient
{
    Task<EmployeeIdDto> GetEmployeeId(EmployeeEmailDto dto);
}