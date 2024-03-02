using wg.modules.owner.domain.Exceptions;
using wg.modules.owner.domain.ValueObjects.Owner;
using wg.modules.owner.domain.ValueObjects.User;
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

    public Owner(Name name, IEnumerable<User> users)
    {
        Name = name;
        _users = users.ToHashSet();
    }

    public static Owner Create(string name)
        => new Owner(name);

    public void AddUser(Guid id, string email, string firstName, string lastName, string password,
        string role)
    {
        if (!_users.Any() && role!= Role.Manager().Value)
        {
            throw new InvalidFirstUserRoleException(id);
        }

        if (_users.Any(x => x.Id.Value == id))
        {
            throw new UserAlreadyRegisteredException(id);
        }
        
        if (_users.Any(x => x.Email == email))
        {
            throw new UserAlreadyRegisteredException(email);
        }

        _users.Add(User.Create(id, email, firstName, lastName, password, role));
    }
}