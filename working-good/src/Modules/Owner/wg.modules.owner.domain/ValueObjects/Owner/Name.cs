using wg.modules.owner.domain.Exceptions;

namespace wg.modules.owner.domain.ValueObjects.Owner;

public record Name
{
    public string Value { get; }

    internal Name(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new EmptyOwnerNameException();
        }
        Value = value;
    }

    public static implicit operator string(Name name)
        => name.Value;

    public static implicit operator Name(string value)
        => new Name(value);
}