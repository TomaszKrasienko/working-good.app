using System;
using wg.shared.abstractions.Exceptions;

namespace wg.modules.tickets.domain.Exceptions;

public class TicketHasNoStatusToChangeActivityException(Guid id) 
    : WgException($"Ticket with ID {id} has no status to change activity");