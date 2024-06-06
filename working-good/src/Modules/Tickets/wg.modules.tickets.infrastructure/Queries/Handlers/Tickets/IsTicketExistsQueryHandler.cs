using Microsoft.EntityFrameworkCore;
using wg.modules.tickets.application.CQRS.Tickets.Queries;
using wg.modules.tickets.application.DTOs;
using wg.modules.tickets.infrastructure.DAL;
using wg.shared.abstractions.CQRS.Queries;

namespace wg.modules.tickets.infrastructure.Queries.Handlers.Tickets;

internal sealed class IsTicketExistsQueryHandler(
    TicketsDbContext dbContext) : IQueryHandler<IsTicketExistsQuery, IsExistsDto>
{
    public async Task<IsExistsDto> HandleAsync(IsTicketExistsQuery query, CancellationToken cancellationToken)
        => new IsExistsDto()
        {
            Value = await dbContext
                .Tickets
                .AsNoTracking()
                .AnyAsync(x => x.Id.Equals(query.Id), cancellationToken)
        };
}