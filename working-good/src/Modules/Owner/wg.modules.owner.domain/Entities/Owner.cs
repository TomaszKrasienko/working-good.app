using wg.modules.owner.domain.Exceptions;
using wg.modules.owner.domain.ValueObjects.User;
using wg.shared.abstractions.Kernel.Types;
using wg.shared.abstractions.Kernel.ValueObjects;

namespace wg.modules.owner.domain.Entities;

public sealed class Owner : AggregateRoot
{
    public Name Name { get; private set; }
    
    private readonly HashSet<User> _users = new HashSet<User>();
    public IEnumerable<User> Users => _users;

    private Owner(AggregateId id, Name name, IEnumerable<User> users)
    {
        Id = id;
        Name = name;
        _users = users.ToHashSet();
    }

    private Owner(AggregateId id, Name name)
    {
        Id = id;
        ChangeName(name);
    }

    public static Owner Create(Guid id, string name)
        => new Owner(id, name);
    
    public void ChangeName(string name)
        => Name = name;

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

    public void VerifyUser(string token, DateTime verificationDateTime)
    {
        var user = _users.FirstOrDefault(x => x.VerificationToken.Token == token);
        if (user is null)
        {
            throw new VerificationTokenNotFoundException(token);
        }

        user.Verify(verificationDateTime);
    }

    public bool IsUserActive(string email)
    {
        var user = _users.FirstOrDefault(x => x.Email == email);
        if (user is null)
        {
            return false;
        }

        if (user.State != State.Activate())
        {
            return false;
        }

        return true;
    }

}