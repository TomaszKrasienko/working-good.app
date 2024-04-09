using wg.shared.abstractions.Exceptions;

namespace wg.modules.tickets.domain.Exceptions;

public sealed class EmptyNoteException() 
    : WgException("Note can not be empty");