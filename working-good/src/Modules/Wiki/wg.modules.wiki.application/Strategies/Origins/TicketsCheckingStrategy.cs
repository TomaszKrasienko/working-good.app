using wg.modules.wiki.application.Clients.Tickets;
using wg.modules.wiki.application.Clients.Tickets.DTOs;
using wg.modules.wiki.application.Exceptions;
using wg.modules.wiki.domain.ValueObjects.Note;

namespace wg.modules.wiki.application.Strategies.Origins;

internal sealed class TicketsCheckingStrategy(
    ITicketsApiClient ticketsApiClient) : IOriginCheckingStrategy
{
    public bool CanByApply(string originType)
        => originType == Origin.Ticket();

    public async Task<bool> IsExists(string originId)
    {
        if (!Guid.TryParse(originId, out var id))
        {
            throw new OriginIdIsInvalidException(originId);
        }

        var dto = new TicketIdDto(id);
        var result = await ticketsApiClient.IsTicketExistsAsync(dto);
        return result.Value;
    }
}