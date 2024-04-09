using System;
using wg.modules.tickets.domain.Exceptions;

namespace wg.modules.tickets.domain.ValueObjects.Activity;

public record ActivityTime
{
    public DateTime TimeFrom { get; }
    public DateTime TimeTo { get; }
    public TimeSpan Summary => TimeTo - TimeFrom;

    public ActivityTime(DateTime timeFrom, DateTime timeTo)
    {
        if (DateTime.Compare(timeFrom, timeTo) > 0)
        {
            throw new TimeToCanNotBeEarlierThanTimeFromException(timeFrom, timeTo);
        }
        TimeFrom = timeFrom;
        TimeTo = timeTo;
    }
}