using Microsoft.EntityFrameworkCore;
using wg.modules.tickets.application.CQRS.Messages.Queries;
using wg.modules.tickets.application.DTOs;
using wg.modules.tickets.infrastructure.DAL;
using wg.modules.tickets.infrastructure.Queries.Mappers;
using wg.shared.abstractions.CQRS.Queries;

namespace wg.modules.tickets.infrastructure.Queries.Handlers.Messages;

internal sealed class GetMessageByIdQueryHandler(
    TicketsDbContext dbContext) : IQueryHandler<GetMessageByIdQuery, MessageDto>
{
    public async Task<MessageDto> HandleAsync(GetMessageByIdQuery query, CancellationToken cancellationToken)
        => (await dbContext
                .Messages
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id.Equals(query.Id), cancellationToken))
            .AsDto();
}