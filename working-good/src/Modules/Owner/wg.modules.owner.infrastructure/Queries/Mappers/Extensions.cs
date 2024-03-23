using wg.modules.owner.application.DTOs;
using wg.modules.owner.domain.Entities;

namespace wg.modules.owner.infrastructure.Queries.Mappers;

internal static class Extensions
{
    internal static OwnerDto AsDto(this Owner owner)
        => new OwnerDto
        {
            Id = owner.Id,
            Name = owner.Name
        };
}