using Microsoft.EntityFrameworkCore;
using wg.modules.activities.application.CQRS.Activities.Queries;
using wg.modules.activities.application.DTOs;
using wg.modules.activities.infrastructure.DAL;
using wg.modules.activities.infrastructure.Queries.Mappers;
using wg.shared.abstractions.CQRS.Queries;

namespace wg.modules.activities.infrastructure.Queries.Handlers;

internal sealed class GetActivityByIdQueryHandler(
    ActivitiesDbContext dbContext) : IQueryHandler<GetActivityById, ActivityDto>
{
    public async Task<ActivityDto> HandleAsync(GetActivityById query, CancellationToken cancellationToken)
        => (await dbContext
                .Activities
                .FirstOrDefaultAsync(x => x.Id.Equals(query.Id), cancellationToken))?
            .AsDto();
}