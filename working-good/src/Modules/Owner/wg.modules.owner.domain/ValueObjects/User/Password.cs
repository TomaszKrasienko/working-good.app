using wg.shared.abstractions.Exceptions;

namespace wg.modules.owner.domain.ValueObjects.User;

public sealed record Password
{
    public string Value { get; }
    
    public Password(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new EmptyPasswordException();
        }

        Value = value;
    }

    public static implicit operator Password(string value)
        => new Password(value);

    public static implicit operator string(Password password)
        => password.Value;
}

public sealed class EmptyPasswordException()
    : WgException("Password can not be empty");