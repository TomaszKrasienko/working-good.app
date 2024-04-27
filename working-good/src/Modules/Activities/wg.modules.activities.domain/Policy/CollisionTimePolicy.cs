
using wg.modules.activities.domain.Entities;
using wg.modules.activities.domain.Policy.Abstractions;

namespace wg.modules.activities.domain.Policy;

internal sealed class CollisionTimePolicy : ICollisionTimePolicy
{
    public bool HasCollision(List<Activity> activities, DateTime timeFrom, DateTime? timeTo)
    {
        if (timeTo is null)
        {
            var alreadyWithNull = activities
                .Any(x => x.ActivityTime.TimeTo is null);
            
            var isExistsAfter = activities
                .Any(x => x.ActivityTime.TimeTo > timeFrom);

            return isExistsAfter || alreadyWithNull;
        }
        else
        {
            var alreadyWithNull = activities
                .Any(x => x.ActivityTime.TimeTo is null && x.ActivityTime.TimeFrom < timeTo);

            var earlierWithCollision = activities
                .Any(x => x.ActivityTime.TimeTo > timeFrom && x.ActivityTime.TimeFrom < timeFrom);

            var laterWithCollision = activities
                .Any(x => x.ActivityTime.TimeFrom < timeTo && x.ActivityTime.TimeTo > timeTo);
            
            return alreadyWithNull || earlierWithCollision || laterWithCollision;
        }
    }

    internal static CollisionTimePolicy GetInstance()
        => new CollisionTimePolicy();
}