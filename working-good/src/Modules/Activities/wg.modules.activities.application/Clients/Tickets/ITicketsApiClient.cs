using wg.modules.activities.application.Clients.Tickets.DTOs;

namespace wg.modules.activities.application.Clients.Tickets;

public interface ITicketsApiClient
{
    Task<TicketExistsDto> IsAvailableForChangesTicketExists(TicketIdDto dto);
}