using wg.modules.wiki.core.DTOs;
using wg.modules.wiki.core.Entities;

namespace wg.modules.wiki.core.Mappers;

public static class Extensions
{
    public static SectionDto AsDto(this Section section)
        => new SectionDto()
        {
            Id = section.Id,
            Name = section.Name
        };
}