using wg.shared.abstractions.Exceptions;

namespace wg.modules.wiki.domain.Exceptions;

public class NoteAlreadyBelongsToSection(Guid noteId)
    : WgException($"Note with ID: {noteId} already belongs to section");