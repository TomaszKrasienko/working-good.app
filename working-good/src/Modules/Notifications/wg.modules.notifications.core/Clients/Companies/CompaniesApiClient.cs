using wg.modules.notifications.core.Clients.Companies.DTO;
using wg.shared.abstractions.Modules;

namespace wg.modules.notifications.core.Clients.Companies;

internal sealed class CompaniesApiClient(
    IModuleClient moduleClient) : ICompaniesApiClient
{
    public Task<EmployeeDto> GetActiveEmployeeByIdAsync(EmployeeIdDto dto)
        => moduleClient.SendAsync<EmployeeDto>("companies/employee/active/get", dto);
}