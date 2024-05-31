using Microsoft.AspNetCore.Http.HttpResults;
using wg.modules.wiki.core.ValueObjects.Section;
using wg.shared.abstractions.Kernel.Types;

namespace wg.modules.wiki.core.Entities;

public sealed class Section
{
    public EntityId Id { get; }
    public Name Name { get; private set; }
    public Section Parent { get; private set; }

    private Section(EntityId id)
    {
        Id = id;
    }

    internal static Section Create(Guid id, string name, Section parent = null)
    {
        var section = new Section(id);
        section.ChangeName(name);
        if (parent is not null)
        {
            section.ChangeParent(parent);
        }

        return section;
    }

    private void ChangeName(string name)
        => Name = name;

    private void ChangeParent(Section parent)
        => Parent = parent;

}