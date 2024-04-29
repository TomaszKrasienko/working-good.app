using wg.modules.activities.application.Clients.Tickets;
using wg.modules.activities.application.Clients.Tickets.DTOs;
using wg.shared.abstractions.Modules;

namespace wg.modules.activities.infrastructure.Clients.Tickets;

internal sealed class TicketsApiClient(
    IModuleClient moduleClient) : ITicketsApiClient
{
    public Task<TicketExistsDto> IsAvailableForChangesTicketExists(TicketIdDto dto)
        => moduleClient.SendAsync<TicketExistsDto>("tickets/is-exists/get/available-for-changes", dto);
}