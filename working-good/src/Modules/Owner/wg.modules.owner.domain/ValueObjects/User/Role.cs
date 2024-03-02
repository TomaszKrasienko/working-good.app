using wg.shared.abstractions.Exceptions;

namespace wg.modules.owner.domain.ValueObjects.User;

public sealed record Role
{
    private static IEnumerable<string> AvailableRoles = new[] {"Manager", "User"};

    public string Value { get; }

    internal Role(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new EmptyUserRoleException();
        if (!AvailableRoles.Contains(value))
            throw new UnavailableUserRoleException(value);
        Value = value;
    }

    public static Role Manager() => new Role("Manager");

    public static Role User() => new Role("User");

    public static implicit operator string(Role role)
        => role.Value;

    public static implicit operator Role(string value)
        => new Role(value);
}

internal sealed class UnavailableUserRoleException(string value)
    : WgException($"Role: {value} is unavailable");

public sealed class EmptyUserRoleException()
    : WgException("User role can not be empty");