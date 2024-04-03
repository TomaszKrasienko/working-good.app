using Microsoft.EntityFrameworkCore;
using wg.modules.owner.application.CQRS.Users.Queries;
using wg.modules.owner.application.DTOs;
using wg.modules.owner.infrastructure.DAL;
using wg.modules.owner.infrastructure.Queries.Mappers;
using wg.shared.abstractions.CQRS.Queries;
using wg.shared.abstractions.Pagination;

namespace wg.modules.owner.infrastructure.Queries.Handlers.Users;

internal sealed class GetUsersQueryHandler(
    OwnerDbContext dbContext) : IQueryHandler<GetUsersQuery, PagedList<UserDto>>
{
    public Task<PagedList<UserDto>> HandleAsync(GetUsersQuery query, CancellationToken cancellationToken)
    {
        var result = dbContext
            .Users
            .AsNoTracking()
            .Select(x => x.AsDto());

        return Task.FromResult(PagedList<UserDto>.ToPagedList(result, query.PageNumber, query.PageSize));
    }
}