using wg.modules.wiki.application.Clients.Tickets;
using wg.modules.wiki.application.Clients.Tickets.DTOs;
using wg.shared.abstractions.Modules;

namespace wg.modules.wiki.infrastructure.Clients;

internal sealed class TicketsApiClient(
    IModuleClient moduleClient) : ITicketsApiClient
{
    public Task<IsTicketExistsDto> IsTicketExistsAsync(TicketIdDto dto)
        => moduleClient.SendAsync<IsTicketExistsDto>("tickets/is-exists/get", dto);
}