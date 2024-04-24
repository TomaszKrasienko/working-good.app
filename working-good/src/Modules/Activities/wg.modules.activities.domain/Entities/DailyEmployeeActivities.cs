using wg.modules.activities.domain.ValueObjects;
using wg.shared.abstractions.Kernel.Types;

namespace wg.modules.activities.domain.Entities;

public sealed class DailyEmployeeActivities : AggregateRoot
{
    public Day Day { get; private set; }
    public EntityId UserId { get; private set; }
    private List<Activity> _activities = new List<Activity>();
    public IReadOnlyList<Activity> Activities => _activities;

    private DailyEmployeeActivities(AggregateId id, Day day, EntityId userId)
    {
        Id = id;
        Day = day;
        UserId = userId;
    }

    public static DailyEmployeeActivities Create(Guid id, DateTime day, Guid userId)
        => new DailyEmployeeActivities(id, day, userId);

    public void AddPaidActivity(Guid id, string content, Guid ticketId)
    {
        
    }
}