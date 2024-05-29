using wg.modules.activities.application.DTOs;
using wg.shared.abstractions.CQRS.Queries;

namespace wg.modules.activities.application.CQRS.Activities.Queries;

public sealed record GetActivitiesByTicketIdQueryQuery(Guid TicketId) : IQuery<IReadOnlyCollection<ActivityDto>>;