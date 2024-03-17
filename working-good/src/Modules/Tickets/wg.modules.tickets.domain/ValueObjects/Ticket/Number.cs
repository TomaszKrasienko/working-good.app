using wg.modules.tickets.domain.Exceptions;

namespace wg.modules.tickets.domain.ValueObjects.Ticket;

public sealed record Number
{
    public string Value { get; private set; }

    public Number(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new EmptyNumberException();
        }
        Value = value;
    }

    public static implicit operator Number(string value)
        => new Number(value);

    public static implicit operator string(Number number)
        => number.Value;
}