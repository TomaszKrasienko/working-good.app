namespace wg.modules.tickets.domain.ValueObjects;

public record ExpirationDate(DateTime Value)
{
    public static implicit operator ExpirationDate(DateTime value)
        => new ExpirationDate(value);

    public static implicit operator DateTime(ExpirationDate expirationDate)
        => expirationDate.Value;
};