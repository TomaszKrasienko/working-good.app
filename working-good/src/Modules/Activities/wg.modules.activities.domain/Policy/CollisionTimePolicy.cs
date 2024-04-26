
using wg.modules.activities.domain.Entities;
using wg.modules.activities.domain.Policy.Abstractions;

namespace wg.modules.activities.domain.Policy;

internal sealed class CollisionTimePolicy : ICollisionTimePolicy
{
    public bool HasCollision(List<Activity> activities, DateTime timeFrom, DateTime timeTo)
    {
        //data ma time from tylko
        var isAnyWithAfterTime = activities.Any(x 
            => x.ActivityTime.TimeTo > timeFrom
            || x.ActivityTime.TimeTo is null);

        var isWithCollision = activities
            .Any(x => x.ActivityTime.TimeFrom < timeTo);
        
        return isAnyWithAfterTime || isWithCollision;
    }

    internal static CollisionTimePolicy GetInstance()
        => new CollisionTimePolicy();
}