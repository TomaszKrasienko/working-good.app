using Microsoft.EntityFrameworkCore;
using wg.modules.tickets.application.CQRS.Tickets.Queries;
using wg.modules.tickets.application.DTOs;
using wg.modules.tickets.infrastructure.DAL;
using wg.modules.tickets.infrastructure.Queries.Mappers;
using wg.shared.abstractions.CQRS.Queries;

namespace wg.modules.tickets.infrastructure.Queries.Handlers.Tickets;

internal sealed class GetTicketByIdQueryHandler(
    TicketsDbContext dbContext) : IQueryHandler<GetTicketByIdQuery, TicketDto>
{
    public async Task<TicketDto> HandleAsync(GetTicketByIdQuery query, CancellationToken cancellationToken)
        => (await dbContext
            .Tickets
            .Include(x => x.Messages)
            .Include(x => x.Activities)
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id.Equals(query.Id), cancellationToken))?
            .AsDto();
}