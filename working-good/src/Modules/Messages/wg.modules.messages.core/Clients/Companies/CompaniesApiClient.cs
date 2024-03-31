using wg.modules.messages.core.Clients.Companies.DTO;
using wg.shared.abstractions.Modules;

namespace wg.modules.messages.core.Clients.Companies;

internal sealed class CompaniesApiClient(
    IModuleClient moduleClient) : ICompaniesApiClient
{
    public Task<EmployeeIdDto> GetEmployeeIdAsync(EmployeeEmailDto dto)
        => moduleClient.SendAsync<EmployeeIdDto>("companies/employee/id/get", dto);
}