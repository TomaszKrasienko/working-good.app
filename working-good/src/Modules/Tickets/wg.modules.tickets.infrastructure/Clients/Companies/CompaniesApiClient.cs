using wg.modules.tickets.application.Clients.Companies;
using wg.modules.tickets.application.Clients.Companies.DTO;
using wg.shared.abstractions.Modules;

namespace wg.modules.tickets.infrastructure.Clients.Companies;

internal sealed class CompaniesApiClient(
    IModuleClient moduleClient) : ICompaniesApiClient
{
    public Task<CompanySlaTimeDto> GetSlaTimeByEmployee(Guid employeeId)
    {
        throw new NotImplementedException();
    }

    public Task<bool> IsEmployeeExists(Guid employeeId)
    {
        throw new NotImplementedException();
    }

    public Task<bool> IsProjectExists(EmployeeWithProjectDto dto)
    {
        throw new NotImplementedException();
    }
}