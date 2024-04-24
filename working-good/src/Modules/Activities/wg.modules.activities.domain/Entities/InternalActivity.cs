using wg.modules.activities.domain.ValueObjects;
using wg.shared.abstractions.Kernel.Types;

namespace wg.modules.activities.domain.Entities;

public sealed class InternalActivity : Activity
{
    private InternalActivity(EntityId id, Content content, EntityId ticketId) 
        : base(id, content, ticketId)
    {
    }
}