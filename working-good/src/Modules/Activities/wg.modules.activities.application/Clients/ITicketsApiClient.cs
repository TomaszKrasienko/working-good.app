using wg.modules.activities.application.Clients.DTOs;

namespace wg.modules.activities.application.Clients;

public interface ITicketsApiClient
{
    Task<TicketExistsDto> IsAvailableForChangesTicketExists(TicketIdDto dto);
}