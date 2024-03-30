using Microsoft.EntityFrameworkCore;
using wg.modules.tickets.application.CQRS.Tickets.Queries;
using wg.modules.tickets.application.DTOs;
using wg.modules.tickets.infrastructure.DAL;
using wg.modules.tickets.infrastructure.Queries.Mappers;
using wg.shared.abstractions.CQRS.Queries;

namespace wg.modules.tickets.infrastructure.Queries.Handlers.Tickets;

internal sealed class GetTaskByIdQueryHandler(
    TicketsDbContext dbContext) : IQueryHandler<GetTaskByIdQuery, TicketDto>
{
    public async Task<TicketDto> HandleAsync(GetTaskByIdQuery query, CancellationToken cancellationToken)
        => (await dbContext
            .Tickets
            .Include(x => x.Messages)
            .FirstOrDefaultAsync(x => x.Id.Equals(query.Id), cancellationToken))?
            .AsDto();
}