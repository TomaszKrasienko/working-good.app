using Microsoft.Extensions.DependencyInjection;
using wg.modules.notifications.core.Clients.Companies;
using wg.modules.notifications.core.Clients.Companies.DTO;

namespace wg.modules.notifications.core.Cache.Decorators;

internal sealed class CompaniesApiClientCacheDecorator(
    ICompaniesApiClient companiesApiClient,
    IServiceProvider serviceProvider) : ICompaniesApiClient
{
    public async Task<EmployeeDto> GetEmployeeByIdAsync(EmployeeIdDto dto)
    {
        using var scope = serviceProvider.CreateScope();
        var cacheService = scope.ServiceProvider.GetRequiredService<ICacheService>();
        var cachedEmployee = await cacheService.Get<EmployeeDto>(dto.Id.ToString());
        if (cachedEmployee is not null) return cachedEmployee;
        var employeeDto = await companiesApiClient.GetEmployeeByIdAsync(dto);
        await cacheService.Add(dto.Id.ToString(), employeeDto);
        return employeeDto;
    }
}