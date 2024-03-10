using wg.shared.abstractions.Exceptions;

namespace wg.modules.companies.domain.Exceptions;

public sealed class InvalidDurationTimeException(DateTime plannedStart, DateTime plannedFinish)
    : WgException($"Planned start: {plannedStart} can not be before planned finish: {plannedFinish}");