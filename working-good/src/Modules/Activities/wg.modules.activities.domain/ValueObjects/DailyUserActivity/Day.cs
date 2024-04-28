namespace wg.modules.activities.domain.ValueObjects.DailyUserActivity;

public sealed record Day
{
    public DateTime Value { get; }

    public Day(DateTime value)
    {
        Value = new DateTime(value.Date.Ticks);
    }

    public static implicit operator DateTime(Day day)
        => day.Value;

    public static implicit operator Day(DateTime value)
        => new Day(value);
}