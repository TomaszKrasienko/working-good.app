using wg.modules.tickets.application.DTOs;
using wg.shared.abstractions.CQRS.Queries;

namespace wg.modules.tickets.application.CQRS.Tickets.Queries;

public record IsTicketAvailableForChangesExistsQuery(Guid TicketId) : IQuery<TicketExistsDto>;