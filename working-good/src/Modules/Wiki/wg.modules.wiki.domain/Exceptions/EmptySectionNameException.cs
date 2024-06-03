using wg.shared.abstractions.Exceptions;

namespace wg.modules.wiki.domain.Exceptions;

public sealed class EmptySectionNameException()
    : WgException("Section name can not be empty");