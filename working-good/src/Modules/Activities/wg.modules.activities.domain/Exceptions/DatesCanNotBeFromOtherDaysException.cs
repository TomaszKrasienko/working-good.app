using wg.shared.abstractions.Exceptions;

namespace wg.modules.activities.domain.Exceptions;

public sealed class DatesCanNotBeFromOtherDaysException(DateTime timeFrom, DateTime? timeTo)
    : WgException($"Provided dates: {timeFrom}, {timeTo} can not be from other days");