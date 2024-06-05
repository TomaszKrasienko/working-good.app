using wg.modules.wiki.application.Clients.Tickets.DTOs;

namespace wg.modules.wiki.application.Clients.Tickets;

public interface ITicketsApiClient
{
    Task<IsTicketExistsDto> IsTicketExistsAsync(TicketIdDto dto);
}