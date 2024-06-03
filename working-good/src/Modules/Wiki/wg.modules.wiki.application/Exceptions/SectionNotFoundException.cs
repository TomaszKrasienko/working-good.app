using wg.shared.abstractions.Exceptions;

namespace wg.modules.wiki.core.Exceptions;

public sealed class SectionNotFoundException(Guid sectionId)
    : WgException($"Section with ID: {sectionId} does not exist");