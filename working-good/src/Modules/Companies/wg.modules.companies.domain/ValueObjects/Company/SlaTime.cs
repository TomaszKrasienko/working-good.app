namespace wg.modules.companies.domain.ValueObjects.Company;

public sealed record SlaTime
{
    public TimeSpan Value { get; }

    internal SlaTime(TimeSpan value)
    {
        if (value == TimeSpan.Zero)
        {
            throw new ZeroSlaTimeException();
        }
        Value = value;
    }

    public static implicit operator TimeSpan(SlaTime slaTime)
        => slaTime.Value;

    public static implicit operator SlaTime(TimeSpan value)
        => new SlaTime(value);
}