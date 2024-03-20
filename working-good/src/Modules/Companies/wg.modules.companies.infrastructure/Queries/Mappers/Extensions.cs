using wg.modules.companies.application.DTOs;
using wg.modules.companies.domain.Entities;

namespace wg.modules.companies.infrastructure.Queries.Mappers;

internal static class Extensions
{
    internal static CompanySlaTimeDto AsSlaTimeDto(this Company entity)
        => new CompanySlaTimeDto()
        {
            SlaTime = entity.SlaTime
        };
}