using System;
using wg.shared.abstractions.Exceptions;

namespace wg.modules.tickets.domain.Exceptions;

public sealed class InvalidStateForAssignUserException(Guid ticketId, string state) 
    : WgException($"State: {state} in ticket with ID: {ticketId} is invalid to assign user");