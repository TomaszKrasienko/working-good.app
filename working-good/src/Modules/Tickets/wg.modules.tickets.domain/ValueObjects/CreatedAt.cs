namespace wg.modules.tickets.domain.ValueObjects;

public sealed record CreatedAt(DateTime Value)
{
    public static implicit operator DateTime(CreatedAt createdAt)
        => createdAt.Value;

    public static implicit operator CreatedAt(DateTime value)
        => new CreatedAt(value);
}