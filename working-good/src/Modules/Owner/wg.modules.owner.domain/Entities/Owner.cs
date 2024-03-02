using wg.modules.owner.domain.ValueObjects.Owner;
using wg.shared.abstractions.Kernel.Types;

namespace wg.modules.owner.domain.Entities;

public sealed class Owner : AggregateRoot
{
    public Name Name { get; }
    
    private readonly HashSet<User> _users = new HashSet<User>();
    public IEnumerable<User> Users => _users;

    private Owner(string name)
    {
        Name = name;
    }
}