using wg.modules.activities.application.CQRS.Activities.Queries;
using wg.modules.activities.application.DTOs;
using wg.modules.activities.infrastructure.DAL;
using wg.shared.abstractions.CQRS.Queries;

namespace wg.modules.activities.infrastructure.Queries.Handlers;

internal sealed class GetActivityByIdQueryHandler(
    ActivitiesDbContext dbContext) : IQueryHandler<GetActivityById, ActivityDto>
{
    public Task<ActivityDto> HandleAsync(GetActivityById query, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}