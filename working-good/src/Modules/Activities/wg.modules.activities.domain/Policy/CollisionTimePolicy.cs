using System.Diagnostics;
using wg.modules.activities.domain.Policy.Abstractions;

namespace wg.modules.activities.domain.Policy;

internal sealed class CollisionTimePolicy : ICollisionTimePolicy
{
    public bool HasCollision(List<Activity> activities, DateTime timeFrom, DateTime timeTo)
    {
        //arrange
        return true;
    }
}