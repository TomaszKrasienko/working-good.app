using System;
using wg.shared.abstractions.Exceptions;

namespace wg.modules.tickets.domain.Exceptions;

public sealed class ActivityNotFoundException(Guid id) 
    : WgException($"Activity with ID: {id} does not exist");