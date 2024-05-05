using System.Net;
using wg.modules.owner.domain.Exceptions;
using wg.shared.abstractions.Kernel.Types;
using wg.shared.abstractions.Kernel.ValueObjects;

namespace wg.modules.owner.domain.Entities;

public sealed class Group
{
    public EntityId Id { get; }
    public Title Title { get; private set; }
    private HashSet<User> _users = new HashSet<User>();
    public IEnumerable<User> Users => _users;

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

    internal void ChangeTitle(string title)
        => Title = title;

    internal void AddUser(User user)
    {
        if (_users.Any(x => x.Id.Equals(user.Id)))
        {
            throw new UserAlreadyInGroupException(user.Id, Id);
        }

        _users.Add(user);
    }

    internal void RemoveUser(User user)
        => _users.Remove(user);
    

}