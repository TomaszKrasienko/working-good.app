namespace wg.modules.companies.domain.ValueObjects.Project;

public record DurationTime(DateTime Value)
{
    public static implicit operator DurationTime(DateTime value)
        => new DurationTime(value);

    public static implicit operator DateTime(DurationTime durationTime)
        => durationTime.Value;
}