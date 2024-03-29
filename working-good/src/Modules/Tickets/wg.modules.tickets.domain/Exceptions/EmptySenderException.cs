using wg.shared.abstractions.Exceptions;

namespace wg.modules.tickets.domain.Exceptions;

public class EmptySenderException() 
    : WgException("Sender can not be empty");