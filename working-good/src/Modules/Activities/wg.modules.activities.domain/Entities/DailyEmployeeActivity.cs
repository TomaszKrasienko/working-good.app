using wg.modules.activities.domain.Exceptions;
using wg.modules.activities.domain.Policy;
using wg.modules.activities.domain.ValueObjects.DailyEmployeeActivities;
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
        Day = day;
        UserId = userId;
    }

    public static DailyEmployeeActivity Create(Guid id, DateTime day, Guid userId)
        => new DailyEmployeeActivity(id, day, userId);

    public void AddPaidActivity(Guid id, string content, Guid ticketId, DateTime timeFrom, DateTime? timeTo)
    {
        ValidateCollision(timeFrom, timeTo);
        _activities.Add(PaidActivity.Create(id, content, ticketId, timeFrom, timeTo));
    }
    
    public void AddInternalActivity(Guid id, string content, Guid ticketId, DateTime timeFrom, DateTime? timeTo)
    {
        ValidateCollision(timeFrom, timeTo);
        _activities.Add(PaidActivity.Create(id, content, ticketId, timeFrom, timeTo));
    }

    public void ChangeActivityType(Guid id)
    {
        
    }

    private void ValidateCollision(DateTime timeFrom, DateTime? timeTo)
    {
        var policy = CollisionTimePolicy.GetInstance();
        if (policy.HasCollision(_activities, timeFrom, timeTo))
        {
            throw new ActivityCollisionTimeException();
        }
    }
}