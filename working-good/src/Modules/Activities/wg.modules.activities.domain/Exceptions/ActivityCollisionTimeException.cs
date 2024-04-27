using wg.shared.abstractions.Exceptions;

namespace wg.modules.activities.domain.Exceptions;

public sealed class ActivityCollisionTimeException()
    : WgException("Activity has collision time");