using System;
using wg.shared.abstractions.Exceptions;

namespace wg.modules.tickets.domain.Exceptions;

public sealed class CanNotAddMessageWithoutAssignedEmployeeException(Guid ticketId)
    :   WgException($"Can not add message to ticket without assigned employee");