using wg.modules.tickets.domain.Exceptions;

namespace wg.modules.tickets.domain.ValueObjects;

public sealed record Content
{
    public string Value { get; }

    public Content(string value)
    {
        if (!string.IsNullOrWhiteSpace(value))
        {
            throw new EmptyContentException();
        }
        Value = value;
    }
    
    public static implicit operator Content(string value)
        => new Content(value);

    public static implicit operator string(Content content)
        => content.Value;
}