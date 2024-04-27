using wg.modules.activities.domain.Policy;
using wg.modules.activities.domain.ValueObjects;
using wg.shared.abstractions.Kernel.Types;

namespace wg.modules.activities.domain.Entities;

public sealed class DailyEmployeeActivity : AggregateRoot
{
    public Day Day { get; private set; }
    public EntityId UserId { get; private set; }
    private List<Activity> _activities = new List<Activity>();
    public IReadOnlyList<Activity> Activities => _activities;

    private DailyEmployeeActivity(AggregateId id, Day day, EntityId userId)
    {
        Id = id;
        Day = day;
        UserId = userId;
    }

    public static DailyEmployeeActivity Create(Guid id, DateTime day, Guid userId)
        => new DailyEmployeeActivity(id, day, userId);

    public void AddPaidActivity(Guid id, string content, Guid ticketId, DateTime timeFrom, DateTime? timeTo)
    {
        
    }

    // private bool HasCollision(DateTime timeFrom, DateTime? timeTo)
    // {
    //     var policy = CollisionTimePolicy.GetInstance();
    //     if (policy.HasCollision(_activities, timeFrom, timeTo))
    //     {
    //         
    //     }
    // }
}