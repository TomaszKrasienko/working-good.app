using System;
using wg.shared.abstractions.Exceptions;

namespace wg.modules.tickets.domain.Exceptions;

public sealed class TicketHasNoStatusToAddActivityException(Guid ticketId)
    : WgException($"Ticket with ID: {ticketId} has no status to add activity");