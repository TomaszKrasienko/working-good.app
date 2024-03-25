using wg.modules.messages.core.Clients.Employees.DTO;
using wg.shared.abstractions.Modules;

namespace wg.modules.messages.core.Clients.Employees;

internal sealed class CompaniesApiClient(
    IModuleClient moduleClient) : ICompaniesApiClient
{
    public Task<IsEmployeeExistsDto> IsEmployeeExists(EmployeeEmailDto dto)
        => moduleClient.SendAsync<IsEmployeeExistsDto>("companies/employee/is-email-exists/get", dto);
}