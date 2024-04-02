using wg.shared.abstractions.Exceptions;

namespace wg.modules.tickets.application.Exceptions;

public sealed class TicketNumberNotFoundException(int ticketNumber) 
    : WgException($"Ticket number: {ticketNumber} does not found");