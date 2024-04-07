namespace wg.shared.abstractions.Kernel.ValueObjects;

public record IsActive(bool Value)
{
    public static implicit operator IsActive(bool value)
        => new IsActive(value);

    public static implicit operator bool(IsActive isActive)
        => isActive.Value;
}