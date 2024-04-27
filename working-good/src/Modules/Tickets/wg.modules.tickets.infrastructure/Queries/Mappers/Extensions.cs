using wg.modules.tickets.application.DTOs;
using wg.modules.tickets.domain.Entities;

namespace wg.modules.tickets.infrastructure.Queries.Mappers;

internal static class Extensions
{
    internal static MessageDto AsDto(this Message entity)
        => new MessageDto()
        {
             Id = entity.Id,
             Sender = entity.Sender,
             Subject = entity.Subject,
             Content = entity.Content,
            CreatedAt = entity.CreatedAt
        };

    internal static TicketDto AsDto(this Ticket entity)
        => new TicketDto()
        {
            Id = entity.Id,
            Number= entity.Number,
            Subject = entity.Subject,
            Content = entity.Content,
            CreatedAt = entity.CreatedAt,
            CreatedBy = entity.CreatedBy,
            IsPriority = entity.IsPriority,
            State = entity.State.Value,
            StateChangeDate = entity.State.ChangeDate,
            ExpirationDate = entity.ExpirationDate,
            AssignedEmployee = entity.AssignedEmployee,
            AssignedUser = entity.AssignedUser,
            ProjectId = entity.ProjectId,
            Messages = entity.Messages?.Select(x => x.AsDto()).ToList()
        };
}