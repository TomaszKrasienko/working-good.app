using System;
using wg.shared.abstractions.Exceptions;

namespace wg.modules.tickets.domain.Exceptions;

public sealed class ActivityHasCollisionDateTimeException(Guid id)
    : WgException($"Ticket with ID: {id} already has activity with collision time");