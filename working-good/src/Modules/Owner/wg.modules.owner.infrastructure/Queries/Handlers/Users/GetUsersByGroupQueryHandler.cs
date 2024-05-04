using System.Collections.Immutable;
using Microsoft.EntityFrameworkCore;
using wg.modules.owner.application.CQRS.Users.Queries;
using wg.modules.owner.application.DTOs;
using wg.modules.owner.infrastructure.DAL;
using wg.modules.owner.infrastructure.Queries.Mappers;
using wg.shared.abstractions.CQRS.Queries;

namespace wg.modules.owner.infrastructure.Queries.Handlers.Users;

internal sealed class GetUsersByGroupQueryHandler(
    OwnerDbContext dbContext) : IQueryHandler<GetUsersByGroupQuery, IEnumerable<UserDto>>
{
    public async Task<IEnumerable<UserDto>> HandleAsync(GetUsersByGroupQuery query, CancellationToken cancellationToken)
        => await dbContext
            .Groups
            .Include(x => x.Users)
            .AsNoTracking()
            .Where(x => x.Id.Equals(query.GroupId))
            .SelectMany(x => x.Users.Select(u => u.AsDto()))
            .ToListAsync(cancellationToken);
}