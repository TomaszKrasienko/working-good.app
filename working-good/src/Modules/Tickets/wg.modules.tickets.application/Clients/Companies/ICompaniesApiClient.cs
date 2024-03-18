using wg.modules.tickets.application.Clients.Companies.DTO;

namespace wg.modules.tickets.application.Clients.Companies;

public interface ICompaniesApiClient
{
    Task<CompanySlaTimeDto> GetSlaTimeByEmployee(Guid employeeId);
    Task<bool> IsEmployeeExists(Guid employeeId);
    Task<bool> IsProjectExists(Guid projectId);
}