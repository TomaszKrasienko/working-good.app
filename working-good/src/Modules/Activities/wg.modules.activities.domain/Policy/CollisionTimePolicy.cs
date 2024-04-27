
using wg.modules.activities.domain.Entities;
using wg.modules.activities.domain.Policy.Abstractions;

namespace wg.modules.activities.domain.Policy;

internal sealed class CollisionTimePolicy : ICollisionTimePolicy
{
    public bool HasCollision(List<Activity> activities, DateTime timeFrom, DateTime? timeTo)
    {
        if (timeTo is null)
        {
            return activities
                .Any(x => (x.ActivityTime.TimeTo is null) || (x.ActivityTime.TimeTo > timeFrom));
        }
        else
        {
            return activities
                .Any(x 
                    => (x.ActivityTime.TimeTo is null && x.ActivityTime.TimeFrom < timeTo)
                    || (x.ActivityTime.TimeTo > timeFrom && x.ActivityTime.TimeFrom < timeFrom)
                    || (x.ActivityTime.TimeFrom < timeTo && x.ActivityTime.TimeTo > timeTo));
        }
    }

    internal static CollisionTimePolicy GetInstance()
        => new CollisionTimePolicy();
}