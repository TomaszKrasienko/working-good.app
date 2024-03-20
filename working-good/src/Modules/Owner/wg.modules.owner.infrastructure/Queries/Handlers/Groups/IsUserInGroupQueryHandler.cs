using Microsoft.EntityFrameworkCore;
using wg.modules.owner.application.CQRS.Groups.Queries;
using wg.modules.owner.application.DTOs;
using wg.modules.owner.infrastructure.DAL;
using wg.shared.abstractions.CQRS.Queries;

namespace wg.modules.owner.infrastructure.Queries.Handlers.Groups;

internal sealed class IsUserInGroupQueryHandler(
    OwnerDbContext dbContext) : IQueryHandler<IsUserInGroupQuery, IsUserInGroupDto>
{
    public async Task<IsUserInGroupDto> HandleAsync(IsUserInGroupQuery query, CancellationToken cancellationToken)
        => new IsUserInGroupDto()
        {
            Value = await dbContext
                .Groups
                .AsNoTracking()
                .AnyAsync(g => g.Id.Equals(query.GroupId) 
                               && g.Users.Any(u => u.Id.Equals(query.UserId)), cancellationToken)
        };
}