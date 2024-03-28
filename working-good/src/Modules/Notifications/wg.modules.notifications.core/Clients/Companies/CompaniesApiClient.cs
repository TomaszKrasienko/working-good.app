using wg.modules.notifications.core.Clients.Companies.DTO;
using wg.shared.abstractions.Modules;

namespace wg.modules.notifications.core.Clients.Companies;

internal sealed class CompaniesApiClient(
    IModuleClient moduleClient) : ICompaniesApiClient
{
    public async Task<EmployeeEmailDto> GetEmployeeEmail(EmployeeIdDto dto)
        => await moduleClient.SendAsync<EmployeeEmailDto>("companies/employee/email/get", dto);
}