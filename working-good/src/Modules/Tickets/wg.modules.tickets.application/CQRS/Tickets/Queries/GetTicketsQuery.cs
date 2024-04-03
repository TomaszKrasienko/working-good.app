using wg.modules.tickets.application.DTOs;
using wg.shared.abstractions.CQRS.Queries;
using wg.shared.abstractions.Pagination;

namespace wg.modules.tickets.application.CQRS.Tickets.Queries;

public sealed record GetTicketsQuery : PaginationDto, IQuery<PagedList<TicketDto>>
{
    public int? TicketNumber { get; init; }
    public string Subject { get; init; }
} 