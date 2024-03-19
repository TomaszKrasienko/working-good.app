using wg.modules.tickets.domain.Entities;

namespace wg.modules.tickets.application.Events.Mappers;

internal static class Extensions
{
    public static TicketCreated AsEvent(this Ticket ticket)
        => new TicketCreated(ticket.Id, ticket.Number, ticket.Subject, ticket.Content,
            ticket.AssignedUser, ticket.AssignedEmployee);
}