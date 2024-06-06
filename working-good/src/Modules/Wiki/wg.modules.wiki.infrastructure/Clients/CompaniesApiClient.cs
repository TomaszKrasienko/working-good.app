using wg.modules.wiki.application.Clients.Companies;
using wg.modules.wiki.application.Clients.Companies.DTOs;
using wg.shared.abstractions.Modules;

namespace wg.modules.wiki.infrastructure.Clients;

internal sealed class CompaniesApiClient(
    IModuleClient moduleClient): ICompaniesApiClient
{
    public Task<IsActiveCompanyExistsDto> IsActiveCompanyExistsAsync(CompanyIdDto dto)
        => moduleClient.SendAsync<IsActiveCompanyExistsDto>("companies/is-exists/get", dto);
}