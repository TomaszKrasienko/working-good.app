using wg.modules.wiki.domain.Exceptions;

namespace wg.modules.wiki.domain.ValueObjects.Section;

public record Name
{
    public string Value { get; }

    public Name(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new EmptySectionNameException();
        }
        Value = value;
    }

    public static implicit operator string(Name name)
        => name.Value;

    public static implicit operator Name(string value)
        => new Name(value);
}