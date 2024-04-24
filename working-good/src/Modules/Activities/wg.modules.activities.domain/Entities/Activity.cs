using wg.shared.abstractions.Kernel.Types;

namespace wg.modules.activities.domain.Entities;

public sealed class Activity 
{
    public EntityId Id { get; private set; }
    public string Content { get; private set; }
    public EntityId TicketId { get; private set; }
}