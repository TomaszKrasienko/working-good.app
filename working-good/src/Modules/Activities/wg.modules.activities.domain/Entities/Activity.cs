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

    protected Activity(EntityId id, Content content, EntityId ticketId, ActivityTime activityTime)
    {
        Id = id;
        Content = content;
        TicketId = ticketId;
        ActivityTime = activityTime;
    }

    protected void ChangeActivityTime(DateTime timeFrom, DateTime timeTo)
    {
        
    }
}