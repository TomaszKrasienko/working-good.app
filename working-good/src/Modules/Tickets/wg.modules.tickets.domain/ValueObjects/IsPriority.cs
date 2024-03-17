namespace wg.modules.tickets.domain.ValueObjects;

public record IsPriority(bool Value)
{
    public static implicit operator IsPriority(bool value)
        => new IsPriority(value);

    public static implicit operator bool(IsPriority isPriority)
        => isPriority.Value;
}