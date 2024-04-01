namespace wg.modules.tickets.domain.ValueObjects.Message;

public record IsStarter(bool Value)
{
    public static implicit operator bool(IsStarter isStarter)
        => isStarter.Value;

    public static implicit operator IsStarter(bool value)
        => new IsStarter(value);
}