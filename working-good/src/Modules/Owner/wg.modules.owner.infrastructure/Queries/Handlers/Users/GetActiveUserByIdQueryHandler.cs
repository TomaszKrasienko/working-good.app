using Microsoft.EntityFrameworkCore;
using wg.modules.owner.application.CQRS.Users.Queries;
using wg.modules.owner.application.DTOs;
using wg.modules.owner.domain.ValueObjects.User;
using wg.modules.owner.infrastructure.DAL;
using wg.modules.owner.infrastructure.Queries.Mappers;
using wg.shared.abstractions.CQRS.Queries;

namespace wg.modules.owner.infrastructure.Queries.Handlers.Users;

internal sealed class GetActiveUserByIdQueryHandler(
    OwnerDbContext dbContext) : IQueryHandler<GetActiveUserByIdQuery, UserDto>
{
    public async Task<UserDto> HandleAsync(GetActiveUserByIdQuery query, CancellationToken cancellationToken)
        => (await dbContext
                .Users
                .AsNoTracking()
                .FirstOrDefaultAsync(x 
                    => x.Id.Equals(query.Id)
                    && x.State == State.Activate(), cancellationToken))?
            .AsDto();
}