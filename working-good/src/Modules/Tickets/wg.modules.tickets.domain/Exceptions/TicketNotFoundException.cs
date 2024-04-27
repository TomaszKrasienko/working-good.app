using System;
using wg.shared.abstractions.Exceptions;

namespace wg.modules.tickets.domain.Exceptions;

public sealed class TicketNotFoundException(Guid ticketId) 
    : WgException($"Ticket with ID: {ticketId} not found");
    