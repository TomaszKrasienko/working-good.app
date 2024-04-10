using System;
using wg.modules.tickets.domain.Exceptions;

namespace wg.modules.tickets.domain.ValueObjects.Activity;

public record ActivityTime
{
    public DateTime TimeFrom { get; }
    public DateTime? TimeTo { get; }
    public TimeSpan Summary => (TimeSpan)(TimeTo is null ? TimeSpan.Zero : TimeTo - (DateTime)TimeFrom);

    public ActivityTime(DateTime timeFrom, DateTime timeTo)
    {
        if (DateTime.Compare(timeFrom, timeTo) > 0)
        {
            throw new TimeToCanNotBeEarlierThanTimeFromException(timeFrom, timeTo);
        }
        TimeFrom = timeFrom;
        TimeTo = timeTo;
    }
    
    public ActivityTime(DateTime timeFrom)
    {
        TimeFrom = timeFrom;
    }
}