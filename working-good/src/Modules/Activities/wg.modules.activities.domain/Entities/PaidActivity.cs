using wg.modules.activities.domain.ValueObjects;
using wg.modules.activities.domain.ValueObjects.Activity;
using wg.shared.abstractions.Kernel.Types;

namespace wg.modules.activities.domain.Entities;

public sealed class PaidActivity : Activity
{
    private PaidActivity(EntityId id, Content content, EntityId ticketId, ActivityTime activityTime) 
        : base(id, content, ticketId, activityTime)
    {
    }

    internal static PaidActivity Create(Guid id, string content, Guid ticketId, DateTime timeFrom, DateTime? timeTo)
    {
        
    }
}