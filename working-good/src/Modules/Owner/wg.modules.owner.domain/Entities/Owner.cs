using wg.modules.owner.domain.Exceptions;
using wg.modules.owner.domain.ValueObjects.User;
using wg.shared.abstractions.Kernel.Types;
using wg.shared.abstractions.Kernel.ValueObjects;

namespace wg.modules.owner.domain.Entities;

public sealed class Owner : AggregateRoot<AggregateId>
{
    public Name Name { get; private set; }
    
    private readonly HashSet<User> _users = new HashSet<User>();
    public IEnumerable<User> Users => _users;

    private readonly HashSet<Group> _groups = new HashSet<Group>();
    public IEnumerable<Group> Groups => _groups;
    
    private Owner(AggregateId id)
    {
        Id = id;
    }

    public static Owner Create(Guid id, string name)
    {   
        var owner = new Owner(id);
        owner.ChangeName(name);
        return owner;
    }
    
    public void ChangeName(string name)
        => Name = name;

    public User AddUser(Guid id, string email, string firstName, string lastName, string password,
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

        var user = User.Create(id, email, firstName, lastName, password, role);
        _users.Add(user);
        return user;
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

    public void EditUser(Guid userId, string email, string firstName, string lastName, string role)
    {
        var user = _users.FirstOrDefault(x => x.Id.Equals(userId));
        if (user is null)
        {
            throw new UserNotFoundException(userId);
        }

        if (_users.All(x => x.Id.Equals(userId)) && role == Role.User())
        {
            throw new InvalidFirstUserRoleException(userId);
        }

        user.ChangeEmail(email);
        user.ChangeFullName(firstName, lastName);
        user.ChangeRole(role);
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

    public void AddGroup(Guid id, string title)
    {
        if (_groups.Any(x => x.Title == title))
        {
            throw new GroupAlreadyExistsException(title);
        }
        _groups.Add(Group.Create(id, title));
    }

    public void AddUserToGroup(Guid groupId, Guid userId)
    {
        var user = _users.FirstOrDefault(x => x.Id.Equals(userId));
        if (user is null)
        {
            throw new UserNotFoundException(userId);
        }

        var group = _groups.FirstOrDefault(x => x.Id.Equals(groupId));
        if (group is null)
        {
            throw new GroupNotFoundException(groupId);
        }

        group.AddUser(user);
    }

    public void EditGroup(Guid id, string title)
    {
        var group = Groups.FirstOrDefault(x => x.Id.Equals(id));
        if (group is null)
        {
            throw new GroupNotFoundException(id);
        }
        
        group.ChangeTitle(title);
    }

    public void DeactivateUser(Guid userId)
    {
        var user = _users.FirstOrDefault(x => x.Id.Equals(userId));
        if (user is null)
        {
            throw new UserNotFoundException(userId);
        }

        var groupsWithUser = _groups.Where(x => x.Users.Contains(user)).ToList();
        groupsWithUser.ForEach(x => x.RemoveUser(user));
         
        user.Deactivate();
    }
    
    
}