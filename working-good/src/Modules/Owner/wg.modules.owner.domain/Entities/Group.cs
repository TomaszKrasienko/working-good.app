using System.Net;
using wg.shared.abstractions.Kernel.Types;
using wg.shared.abstractions.Kernel.ValueObjects;

namespace wg.modules.owner.domain.Entities;

public sealed class Group
{
    public EntityId Id { get; }
    public Title Title { get; private set; }
    public HashSet<User> _users = new HashSet<User>();
    public IEnumerable<User> Users;

    private Group(EntityId id, Title title)
    {
        Id = id;
        Title = title;
    }

    private Group(EntityId id)
        => Id = id;

    internal static Group Create(Guid id, string title)
    {
        var group = new Group(id);
        group.ChangeTitle(title);
        return group;
    }

    private void ChangeTitle(string title)
        => Title = title;

}