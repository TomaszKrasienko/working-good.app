using wg.modules.companies.domain.ValueObjects.Project;
using wg.shared.abstractions.Kernel.Types;

namespace wg.modules.companies.domain.Entities;

public sealed class Project
{
    public EntityId Id { get; }
    public Title Title { get; }
    public Description Description { get; }
    public DurationTime PlannedStart { get; }
    public DurationTime PlannedFinish { get; }
}