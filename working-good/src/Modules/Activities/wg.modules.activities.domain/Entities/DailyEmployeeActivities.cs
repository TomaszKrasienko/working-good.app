using wg.shared.abstractions.Kernel.Types;

namespace wg.modules.activities.domain.Entities;

public sealed class DailyEmployeeActivities : AggregateRoot
{
    public DateTime Day { get; set; }
    public EntityId EmployeeId { get; set; }
}