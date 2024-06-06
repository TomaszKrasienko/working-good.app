using wg.modules.wiki.application.DTOs;
using wg.modules.wiki.domain.Entities;

namespace wg.modules.wiki.infrastructure.Queries.Mappers;

public static class Extensions
{
    public static SectionDto AsDto(this Section section)
        => new SectionDto()
        {
            Id = section.Id,
            Name = section.Name,
            Notes = section.Notes?.Select(x => x.AsDto()).ToList()
        };

    public static NoteDto AsDto(this Note note)
        => new NoteDto()
        {
            Id = note?.Id,
            Title = note?.Title,
            Content = note?.Content,
            OriginId = note?.Origin?.Id,
            OriginType = note?.Origin?.Type
        };
}