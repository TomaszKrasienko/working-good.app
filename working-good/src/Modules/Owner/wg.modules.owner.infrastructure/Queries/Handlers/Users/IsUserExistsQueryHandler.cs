using Microsoft.EntityFrameworkCore;
using wg.modules.owner.application.CQRS.Users.Queries;
using wg.modules.owner.application.DTOs;
using wg.modules.owner.infrastructure.DAL;
using wg.shared.abstractions.CQRS.Queries;

namespace wg.modules.owner.infrastructure.Queries.Handlers.Users;

internal sealed class IsUserExistsQueryHandler(
    OwnerDbContext dbContext) : IQueryHandler<IsUserExistsQuery, IsExistsDto>
{
    public async Task<IsExistsDto> HandleAsync(IsUserExistsQuery query, CancellationToken cancellationToken)
        => new IsExistsDto()
        {
            Value = await dbContext
                    .Users
                    .AsNoTracking()
                    .AnyAsync(x => x.Id.Equals(query.Id), cancellationToken) 
        };
}