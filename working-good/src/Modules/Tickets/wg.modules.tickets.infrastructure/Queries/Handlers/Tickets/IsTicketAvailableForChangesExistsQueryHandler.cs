using Microsoft.EntityFrameworkCore;
using wg.modules.tickets.application.CQRS.Tickets.Queries;
using wg.modules.tickets.application.DTOs;
using wg.modules.tickets.domain.ValueObjects.Ticket;
using wg.modules.tickets.infrastructure.DAL;
using wg.shared.abstractions.CQRS.Queries;

namespace wg.modules.tickets.infrastructure.Queries.Handlers.Tickets;

internal sealed class IsTicketAvailableForChangesExistsQueryHandler(
    TicketsDbContext dbContext) : IQueryHandler<IsTicketAvailableForChangesExistsQuery, IsExistsDto>
{
    public async Task<IsExistsDto> HandleAsync(IsTicketAvailableForChangesExistsQuery query,
        CancellationToken cancellationToken)
        => new IsExistsDto()
        {
            Value = await dbContext
                .Tickets
                .AsNoTracking()
                .AnyAsync(x => Status.AvailableForChangesStatuses.Contains(x.Status.Value),
                    cancellationToken)
        };
}