using wg.modules.wiki.application.Clients.Companies;
using wg.modules.wiki.application.Clients.Companies.DTOs;
using wg.modules.wiki.application.Exceptions;
using wg.modules.wiki.domain.ValueObjects.Note;

namespace wg.modules.wiki.application.Strategies.Origins;

internal sealed class ClientCheckingStrategy(
    ICompaniesApiClient companiesApiClient) : IOriginCheckingStrategy
{
    public bool CanByApply(string originType)
        => originType == Origin.Client();

    public async Task<bool> IsExists(string originId)
    {
        if (!Guid.TryParse(originId, out var id))
        {
            throw new OriginIdIsInvalidException(originId);
        }

        var dto = new CompanyIdDto(id);
        var result = await companiesApiClient.IsActiveCompanyExistsAsync(dto);
        return result.Value;
    }
}