using wg.modules.tickets.application.DTOs;
using wg.modules.tickets.domain.Entities;

namespace wg.modules.tickets.infrastructure.Queries.Mappers;

internal static class Extensions
{
    internal static MessageDto AsDto(this Message dto)
        => new MessageDto()
        {
             Id = dto.Id,
             Sender = dto.Sender,
             Subject = dto.Subject,
             Content = dto.Content,
            CreatedAt = dto.CreatedAt
        };
}