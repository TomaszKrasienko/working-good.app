using wg.modules.activities.domain.Exceptions;

namespace wg.modules.activities.domain.ValueObjects.Activity;

public sealed record ActivityTime
{
    public DateTime TimeFrom { get; }
    public DateTime? TimeTo { get; }
    public TimeSpan Summary => (TimeSpan)(TimeTo is null ? TimeSpan.Zero : TimeTo - (DateTime)TimeFrom);

    public ActivityTime(DateTime timeFrom, DateTime timeTo)
    {
        if (timeFrom.Date != timeTo.Date)
        {
            throw new DatesCanNotBeFromOtherDaysException(timeFrom, timeTo);
        }
        
        if (DateTime.Compare(timeFrom, timeTo) > 0)
        {
            throw new TimeToCanNotBeEarlierThanTimeFromException(timeFrom, timeTo);
        }
        TimeFrom = new DateTime(timeFrom.Year, timeFrom.Month, timeFrom.Day, timeFrom.Hour, timeFrom.Minute, 0);
        TimeTo = new DateTime(timeTo.Year, timeTo.Month, timeTo.Day, timeTo.Hour, timeTo.Minute, 0);;
    }
    
    public ActivityTime(DateTime timeFrom)
    {
        TimeFrom = timeFrom;
    }
}