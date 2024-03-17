using wg.modules.tickets.domain.Exceptions;

namespace wg.modules.tickets.domain.ValueObjects;

public sealed record Subject
{
    public string Value { get; }

    public Subject(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new EmptySubjectException();
        }
        Value = value;
    }

    public static implicit operator Subject(string value)
        => new Subject(value);

    public static implicit operator string(Subject subject)
        => subject.Value;
}