using wg.modules.activities.application.DTOs;
using wg.modules.activities.domain.Entities;

namespace wg.modules.activities.infrastructure.Queries.Mappers;

internal static class Extensions
{
    internal static ActivityDto AsDto(this Activity activity)
        => new ActivityDto()
        {
            Id = activity.Id, 
            Content = activity.Content,
            TimeFrom = activity.ActivityTime.TimeFrom,
            TimeTo = activity.ActivityTime.TimeTo,
            Summary = activity.ActivityTime.Summary
        };
}