using wg.modules.wiki.core.Exceptions;

namespace wg.modules.wiki.core.ValueObjects.Note;

public sealed record Title
{
    public string Value { get; }

    public Title(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new EmptyNoteTitleException();
        }
        Value = value;
    }

    public static implicit operator string(Title title)
        => title.Value;

    public static implicit operator Title(string value)
        => new Title(value);
}