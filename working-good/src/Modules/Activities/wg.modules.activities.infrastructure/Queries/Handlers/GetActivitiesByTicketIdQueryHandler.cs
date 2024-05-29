using Microsoft.EntityFrameworkCore;
using wg.modules.activities.application.CQRS.Activities.Queries;
using wg.modules.activities.application.DTOs;
using wg.modules.activities.infrastructure.DAL;
using wg.modules.activities.infrastructure.Queries.Mappers;
using wg.shared.abstractions.CQRS.Queries;

namespace wg.modules.activities.infrastructure.Queries.Handlers;

internal sealed class GetActivitiesByTicketIdQueryHandler(
    ActivitiesDbContext context) : IQueryHandler<GetActivitiesByTicketIdQueryQuery, IReadOnlyCollection<ActivityDto>>
{
    public async Task<IReadOnlyCollection<ActivityDto>> HandleAsync(GetActivitiesByTicketIdQueryQuery query,
        CancellationToken cancellationToken)
        => await context
            .Activities
            .AsNoTracking()
            .Where(x => x.TicketId.Equals(query.TicketId))
            .Select(x => x.AsDto())
            .ToListAsync(cancellationToken);
}