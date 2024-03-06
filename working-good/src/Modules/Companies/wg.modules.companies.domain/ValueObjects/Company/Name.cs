namespace wg.modules.companies.domain.ValueObjects.Company;

public sealed record Name
{
    public string Value { get; }

    public Name(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            
        }
        Value = value;
    }
}