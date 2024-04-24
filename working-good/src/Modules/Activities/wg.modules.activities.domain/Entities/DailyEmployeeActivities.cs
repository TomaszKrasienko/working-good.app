using wg.modules.activities.domain.ValueObjects;
using wg.shared.abstractions.Kernel.Types;

namespace wg.modules.activities.domain.Entities;

public sealed class DailyEmployeeActivities : AggregateRoot
{
    public Day Day { get; private set; }
    public EntityId EmployeeId { get; private set; }
    private List<Activity> _activities = new List<Activity>();
    public IReadOnlyList<Activity> Activities => _activities;

    private DailyEmployeeActivities(AggregateId id, Day day, EntityId employeeId)
    {
        Id = id;
        Day = day;
        EmployeeId = employeeId;
    }

    public static DailyEmployeeActivities Create(Guid id, DateTime day, Guid employeeId)
        => new DailyEmployeeActivities(id, day, employeeId);
}