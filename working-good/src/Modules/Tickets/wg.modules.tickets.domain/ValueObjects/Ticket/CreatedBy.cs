using wg.modules.tickets.domain.Exceptions;

namespace wg.modules.tickets.domain.ValueObjects.Ticket;

public sealed record CreatedBy
{
    public string Value { get; }

    public CreatedBy(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new EmptyCreatedByException();
        }
        Value = value;
    }

    public static implicit operator string(CreatedBy createdBy)
        => createdBy.Value;

    public static implicit operator CreatedBy(string value)
        => new CreatedBy(value);
}