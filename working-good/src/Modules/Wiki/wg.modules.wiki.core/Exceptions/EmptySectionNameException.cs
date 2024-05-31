using wg.shared.abstractions.Exceptions;

namespace wg.modules.wiki.core.Exceptions;

public sealed class EmptySectionNameException()
    : WgException("Section name can not be empty");