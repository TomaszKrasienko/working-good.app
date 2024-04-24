using wg.modules.activities.domain.Exceptions;

namespace wg.modules.activities.domain.ValueObjects.Activity;

public sealed record Content
{
    public string Value { get; init; }

    internal Content(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new EmptyActivityContentException();
        }
        Value = value;
    }

    public static implicit operator string(Content content)
        => content.Value;

    public static implicit operator Content(string value)
        => new Content(value);
}