using wg.modules.wiki.core.ValueObjects.Section;
using wg.shared.abstractions.Kernel.Types;

namespace wg.modules.wiki.core.Entities;

public sealed class Section
{
    public EntityId Id { get; }
    public Name Name { get; private set; }
    public Section Parent { get; private set; }

    private Section(EntityId id, Name name, Section parent)
    {
        Id = id;
        Name = name;
        Parent = parent;
    }
}