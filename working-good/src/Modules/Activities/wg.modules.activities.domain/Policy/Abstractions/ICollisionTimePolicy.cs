using wg.modules.activities.domain.Entities;

namespace wg.modules.activities.domain.Policy.Abstractions;

public interface ICollisionTimePolicy
{
    bool HasCollision(List<Activity> activities, DateTime timeFrom, DateTime timeTo);
}