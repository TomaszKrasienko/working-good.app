using Microsoft.EntityFrameworkCore;
using wg.modules.owner.application.CQRS.Users.Queries;
using wg.modules.owner.application.DTOs;
using wg.modules.owner.infrastructure.DAL;
using wg.modules.owner.infrastructure.Queries.Mappers;
using wg.shared.abstractions.CQRS.Queries;

namespace wg.modules.owner.infrastructure.Queries.Handlers.Users;

internal sealed class GetUserByIdQueryHandler(
    OwnerDbContext dbContext) : IQueryHandler<GetUserByIdQuery, UserDto>
{
    public async Task<UserDto> HandleAsync(GetUserByIdQuery query, CancellationToken cancellationToken)
        => (await dbContext
                .Users
                .FirstOrDefaultAsync(x => x.Id.Equals(query.Id), cancellationToken))?
            .AsDto();
}