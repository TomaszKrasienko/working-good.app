using wg.shared.abstractions.Exceptions;

namespace wg.modules.wiki.domain.Exceptions;

public sealed class OriginTypeNoteAvailableException(string type) 
    : WgException($"Origin: {type} is unavailable");