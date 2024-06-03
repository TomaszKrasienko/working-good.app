using wg.modules.wiki.core.DTOs;
using wg.modules.wiki.domain.Entities;

namespace wg.modules.wiki.infrastructure.Mappers;

public static class Extensions
{
    public static SectionDto AsDto(this Section section)
        => new SectionDto()
        {
            Id = section.Id,
            Name = section.Name
        };
}