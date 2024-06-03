using wg.shared.abstractions.Exceptions;

namespace wg.modules.wiki.domain.Exceptions;

public sealed class EmptyNoteContentException()
    :  WgException("Note content can not be empty");