using wg.modules.tickets.application.DTOs;
using wg.shared.abstractions.CQRS.Queries;

namespace wg.modules.tickets.application.CQRS.Messages.Queries;

public sealed record GetMessageByIdQuery(Guid Id) : IQuery<MessageDto>;