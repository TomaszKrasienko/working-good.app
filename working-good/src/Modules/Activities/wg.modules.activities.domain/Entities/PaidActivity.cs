using wg.shared.abstractions.Kernel.Types;

namespace wg.modules.activities.domain.Entities;

public sealed class PaidActivity : Activity
{
    private PaidActivity(EntityId id, EntityId ticketId) 
        : base(id, ticketId)
    {
    }

    internal static PaidActivity Create(Guid id, string content, Guid ticketId, DateTime timeFrom, DateTime? timeTo)
    {
        var activity = new PaidActivity(id, ticketId);
        activity.ChangeContent(content);
        activity.ChangeActivityTime(timeFrom, timeTo);
        return activity;
    }
}