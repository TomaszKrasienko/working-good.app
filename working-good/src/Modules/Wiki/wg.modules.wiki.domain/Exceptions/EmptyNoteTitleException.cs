using wg.shared.abstractions.Exceptions;

namespace wg.modules.wiki.domain.Exceptions;

public sealed class EmptyNoteTitleException()
    : WgException("Note title can not be empty");