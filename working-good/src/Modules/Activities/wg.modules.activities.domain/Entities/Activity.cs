using wg.modules.activities.domain.ValueObjects;
using wg.modules.activities.domain.ValueObjects.Activity;
using wg.shared.abstractions.Kernel.Types;

namespace wg.modules.activities.domain.Entities;

public abstract class Activity 
{
    public EntityId Id { get; private set; }
    public Content Content { get; private set; }
    public EntityId TicketId { get; private set; }
    public ActivityTime ActivityTime { get; private set; }

    protected Activity(EntityId id, EntityId ticketId)
    {
        Id = id;
        TicketId = ticketId;
    }

    protected void ChangeContent(string content)
        => Content = content;

    protected virtual void ChangeActivityTime(DateTime timeFrom, DateTime? timeTo)
    {
        if (timeTo is null)
        {
            ActivityTime = new ActivityTime(timeFrom);
            return;
        }
        ActivityTime = new ActivityTime(timeFrom, (DateTime)timeTo);
    }

    internal abstract Activity ChangeType();
}