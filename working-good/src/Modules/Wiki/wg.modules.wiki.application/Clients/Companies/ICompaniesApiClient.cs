using wg.modules.wiki.application.Clients.Companies.DTOs;

namespace wg.modules.wiki.application.Clients.Companies;

public interface ICompaniesApiClient
{
    Task<IsActiveCompanyExistsDto> IsActiveCompanyExistsAsync(CompanyIdDto dto);
}