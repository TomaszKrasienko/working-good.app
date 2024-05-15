using Microsoft.EntityFrameworkCore;
using wg.modules.owner.application.CQRS.Groups.Queries;
using wg.modules.owner.application.DTOs;
using wg.modules.owner.domain.ValueObjects.User;
using wg.modules.owner.infrastructure.DAL;
using wg.shared.abstractions.CQRS.Queries;

namespace wg.modules.owner.infrastructure.Queries.Handlers.Projects;

internal sealed class IsMembershipExistsQueryHandler(
    OwnerDbContext dbContext) : IQueryHandler<IsMembershipExistsQuery, IsExistsDto>
{
    public async Task<IsExistsDto> HandleAsync(IsMembershipExistsQuery query, CancellationToken cancellationToken)
        => new IsExistsDto()
        {
            Value = await dbContext
                .Owner
                .Include(x => x.Users)
                .Include(x => x.Groups)
                .AnyAsync(x
                        => x.Users.Any(y => y.Id.Equals(query.UserId) && y.State == State.Activate())
                           && x.Groups.Any(y =>
                               y.Id.Equals(query.GroupId) && y.Users.Any(u => u.Id.Equals(query.UserId))),
                    cancellationToken)
        };
}