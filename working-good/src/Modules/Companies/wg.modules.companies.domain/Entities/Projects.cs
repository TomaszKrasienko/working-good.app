using wg.shared.abstractions.Kernel.Types;

namespace wg.modules.companies.domain.Entities;

public sealed class Project
{
    public EntityId Id { get; }
    public string Title { get; }
    public string Description { get; }
    public DateTime PlannedStart { get; }
    public DateTime PlannedFinish { get; }
}