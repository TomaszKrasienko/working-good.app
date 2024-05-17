using System;
using wg.shared.abstractions.Exceptions;

namespace wg.modules.tickets.domain.Exceptions;

public sealed class InvalidSlaTimeForTicketException(Guid ticketId, TimeSpan? slaTime)
    : WgException($"SlaTime: {slaTime} is invalid for ticket with ID: {ticketId}");