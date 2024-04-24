using wg.shared.abstractions.Exceptions;

namespace wg.modules.activities.domain.Exceptions;

public sealed class EmptyActivityContentException()
    : WgException("Activity content can not be empty");