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

    public static bool operator ==(Day day, DateTime dateTime)
            => day?.Value.Year == dateTime.Year
           && day.Value.Month == dateTime.Month
           && day.Value.Day == dateTime.Day;
    public static bool operator !=(Day day, DateTime dateTime)
        =>  !(day == dateTime);
}