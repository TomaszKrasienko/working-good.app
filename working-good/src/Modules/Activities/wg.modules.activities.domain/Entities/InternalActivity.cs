using wg.shared.abstractions.Kernel.Types;

namespace wg.modules.activities.domain.Entities;

public sealed class InternalActivity : Activity
{
    private InternalActivity(EntityId id, EntityId ticketId) 
        : base(id, ticketId)
    {
    }
}