using wg.shared.abstractions.Exceptions;

namespace wg.modules.activities.application.Exceptions;

public class TicketWithStateForChangesNotFoundException(Guid ticketId)
    : WgException($"Ticket with state for changes with ID: {ticketId} not found");