using wg.modules.tickets.domain.Exceptions;

namespace wg.modules.tickets.domain.ValueObjects.Ticket;

public sealed record Number
{
    public int Value { get; private set; }

    public Number(int value)
    {
        if (value < 1)
        {
            throw new InvalidNumberException(value);
        }
        Value = value;
    }

    public static implicit operator Number(int value)
        => new Number(value);

    public static implicit operator int(Number number)
        => number.Value;
}