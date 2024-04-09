namespace wg.modules.tickets.domain.ValueObjects.Activity;

public record IsPaid(bool Value)
{
    public static implicit operator IsPaid(bool value)
        => new IsPaid(value);

    public static implicit operator bool(IsPaid isPaid)
        => isPaid.Value;
}