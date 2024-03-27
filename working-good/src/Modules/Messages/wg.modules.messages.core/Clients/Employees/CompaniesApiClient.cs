using wg.modules.messages.core.Clients.Employees.DTO;
using wg.shared.abstractions.Modules;

namespace wg.modules.messages.core.Clients.Employees;

internal sealed class CompaniesApiClient(
    IModuleClient moduleClient) : ICompaniesApiClient
{
    public Task<EmployeeIdDto> GetEmployeeId(EmployeeEmailDto dto)
        => moduleClient.SendAsync<EmployeeIdDto>("companies/employee/id/get", dto);
}