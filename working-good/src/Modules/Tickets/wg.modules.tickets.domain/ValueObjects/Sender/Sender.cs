using wg.modules.tickets.domain.Exceptions;

namespace wg.modules.tickets.domain.ValueObjects.Sender;

public sealed record Sender
{
    public string Value { get; }

    public Sender(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new EmptySenderException();
        }
        Value = value;
    }

    public static implicit operator Sender(string value)
        => new Sender(value);

    public static implicit operator string(Sender sender)
        => sender.Value;
}