using wg.modules.messages.core.Clients.Companies.DTO;
using wg.shared.abstractions.Modules;

namespace wg.modules.messages.core.Clients.Companies;

internal sealed class CompaniesApiClient(
    IModuleClient moduleClient) : ICompaniesApiClient
{
    public Task<EmployeeDto> GetEmployeeByEmailAsync(EmployeeEmailDto dto)
        => moduleClient.SendAsync<EmployeeDto>("companies/employee/active/get/by-email", dto);
}