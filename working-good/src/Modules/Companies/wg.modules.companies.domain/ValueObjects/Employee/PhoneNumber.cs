namespace wg.modules.companies.domain.ValueObjects.Employee;

public record PhoneNumber
{
    public string Value { get; set; }

    public PhoneNumber(string value)
    {
        Value = value;
    }

    public static implicit operator string(PhoneNumber phoneNumber)
        => phoneNumber.Value;

    public static implicit operator PhoneNumber(string value)
        => new PhoneNumber(value);
}