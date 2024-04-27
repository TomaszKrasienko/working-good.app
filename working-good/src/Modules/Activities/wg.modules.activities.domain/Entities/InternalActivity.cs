using wg.shared.abstractions.Kernel.Types;

namespace wg.modules.activities.domain.Entities;

public sealed class InternalActivity : Activity
{
    private InternalActivity(EntityId id, EntityId ticketId) 
        : base(id, ticketId)
    {
    }
    
    internal static InternalActivity Create(Guid id, string content, Guid ticketId, DateTime timeFrom, DateTime? timeTo)
    {
        var activity = new InternalActivity(id, ticketId);
        activity.ChangeContent(content);
        activity.ChangeActivityTime(timeFrom, timeTo);
        return activity;
    }

    internal static InternalActivity Create(PaidActivity paidActivity)
    {
        var activity = new InternalActivity(paidActivity.Id, paidActivity.TicketId);
        activity.ChangeContent(paidActivity.Content);
        activity.ChangeActivityTime(paidActivity.ActivityTime.TimeFrom, paidActivity.ActivityTime.TimeTo);
        return activity;
    }

    internal override Activity ChangeType()
        => PaidActivity.Create(this);
}