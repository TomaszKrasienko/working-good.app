using wg.shared.abstractions.Exceptions;

namespace wg.modules.activities.domain.Exceptions;

public class TimeToCanNotBeEarlierThanTimeFromException(DateTime timeFrom, DateTime timeTo)
    : WgException($"Time to: {timeTo} can not be before time from: {timeFrom}");