using wg.shared.abstractions.Exceptions;

namespace wg.modules.tickets.application.Exceptions;

public sealed class TicketAlreadyRegisteredException(Guid id)
    : WgException($"Ticket with ID: {id} is already registered");