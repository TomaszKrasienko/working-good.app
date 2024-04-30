using Microsoft.EntityFrameworkCore;
using wg.modules.tickets.application.CQRS.Tickets.Queries;
using wg.modules.tickets.application.DTOs;
using wg.modules.tickets.domain.ValueObjects.Ticket;
using wg.modules.tickets.infrastructure.DAL;
using wg.shared.abstractions.CQRS.Queries;

namespace wg.modules.tickets.infrastructure.Queries.Handlers.Tickets;

//TODO: Add collection with states

internal sealed class IsTicketAvailableForChangesExistsQueryHandler(
    TicketsDbContext dbContext) : IQueryHandler<IsTicketAvailableForChangesExistsQuery, TicketExistsDto>
{
    public async Task<TicketExistsDto> HandleAsync(IsTicketAvailableForChangesExistsQuery query,
        CancellationToken cancellationToken)
        => new TicketExistsDto()
        {
            Value = await dbContext
                .Tickets
                .AsNoTracking()
                .AnyAsync(x => x.State.Value != State.Cancelled() || x.State.Value != State.Done(),
                    cancellationToken)
        };
}