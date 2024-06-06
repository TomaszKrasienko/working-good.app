using wg.modules.tickets.application.DTOs;
using wg.shared.abstractions.CQRS.Queries;

namespace wg.modules.tickets.application.CQRS.Tickets.Queries;

public sealed record IsTicketExistsQuery(Guid Id) : IQuery<IsExistsDto>;