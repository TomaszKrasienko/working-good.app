using Microsoft.EntityFrameworkCore;
using wg.modules.tickets.application.CQRS.Tickets.Queries;
using wg.modules.tickets.application.DTOs;
using wg.modules.tickets.infrastructure.DAL;
using wg.modules.tickets.infrastructure.Queries.Mappers;
using wg.shared.abstractions.CQRS.Queries;
using wg.shared.abstractions.Pagination;

namespace wg.modules.tickets.infrastructure.Queries.Handlers.Tickets;

internal sealed class GetTicketsQueryHandler(
    TicketsDbContext dbContext) : IQueryHandler<GetTicketsQuery, PagedList<TicketDto>>
{
    public Task<PagedList<TicketDto>> HandleAsync(GetTicketsQuery query, CancellationToken cancellationToken)
    {
        var results = dbContext
            .Tickets
            .AsNoTracking()
            .AsEnumerable()
             .Where(x
                 => (query.TicketNumber == null || x.Number == query.TicketNumber)
                 && (string.IsNullOrWhiteSpace(query.Subject) || x.Subject.Value.Contains(query.Subject)))
            .Select(x => x.AsDto());
        
        return Task.FromResult(PagedList<TicketDto>.ToPagedList(results.AsQueryable(), query.PageNumber, query.PageSize));
    }
}