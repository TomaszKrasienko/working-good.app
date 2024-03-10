namespace wg.modules.companies.domain.ValueObjects.Project;

public sealed record Description(string Value)
{
    public static implicit operator Description(string value)
        => new Description(value);

    public static implicit operator string(Description description)
        => description.Value;
}