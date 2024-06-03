using wg.shared.abstractions.Exceptions;

namespace wg.modules.wiki.application.Exceptions;

public sealed class ParentSectionNotFoundException(Guid parentId)
    : WgException($"Parent section with ID: {parentId} does not exist");