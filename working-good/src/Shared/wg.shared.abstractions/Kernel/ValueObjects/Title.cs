using wg.shared.abstractions.Kernel.Exceptions;

namespace wg.shared.abstractions.Kernel.ValueObjects;

public sealed record Title
{
    public string Value { get; }
    
    public Title(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new EmptyTitleException();
        }

        Value = value;
    }

    public static implicit operator Title(string value)
        => new Title(value);

    public static implicit operator string(Title title)
        => title.Value;
}