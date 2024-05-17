using System;
using wg.shared.abstractions.Exceptions;

namespace wg.modules.tickets.domain.Exceptions;

public sealed class MissingAssignedEmployeeException(Guid ticketId)
    : WgException($"Ticket with ID:{ticketId} can not have not assigned employee to be priority");