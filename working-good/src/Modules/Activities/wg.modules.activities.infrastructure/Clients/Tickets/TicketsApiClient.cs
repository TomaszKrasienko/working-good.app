using wg.modules.activities.application.Clients;
using wg.modules.activities.application.Clients.DTOs;

namespace wg.modules.activities.infrastructure.Clients.Tickets;

internal sealed class TicketsApiClient : ITicketsApiClient
{
    public Task<TicketExistsDto> IsAvailableForChangesTicketExists(TicketIdDto dto)
    {
        throw new NotImplementedException();
    }
}