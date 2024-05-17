using Microsoft.EntityFrameworkCore;
using wg.modules.owner.application.CQRS.Users.Queries;
using wg.modules.owner.application.DTOs;
using wg.modules.owner.domain.ValueObjects.User;
using wg.modules.owner.infrastructure.DAL;
using wg.shared.abstractions.CQRS.Queries;

namespace wg.modules.owner.infrastructure.Queries.Handlers.Users;

internal sealed class IsActiveUserExistsQueryHandler(
    OwnerDbContext ownerDbContext) : IQueryHandler<IsActiveUserExistsQuery, IsExistsDto>
{
    public async Task<IsExistsDto> HandleAsync(IsActiveUserExistsQuery query, CancellationToken cancellationToken)
        => new IsExistsDto()
        {
            Value = await ownerDbContext
                .Users
                .AsNoTracking()
                .AnyAsync(u
                    => u.Id.Equals(query.Id)
                       && u.State == State.Activate(), cancellationToken)
        };
}