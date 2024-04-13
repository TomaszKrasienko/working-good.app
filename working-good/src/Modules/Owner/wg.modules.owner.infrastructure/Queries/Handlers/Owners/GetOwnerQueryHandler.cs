using Microsoft.EntityFrameworkCore;
using wg.modules.owner.application.CQRS.Owners.Queries;
using wg.modules.owner.application.DTOs;
using wg.modules.owner.domain.Entities;
using wg.modules.owner.domain.ValueObjects.User;
using wg.modules.owner.infrastructure.DAL;
using wg.modules.owner.infrastructure.Queries.Mappers;
using wg.shared.abstractions.CQRS.Queries;

namespace wg.modules.owner.infrastructure.Queries.Handlers.Owners;

internal sealed class GetOwnerQueryHandler
    (OwnerDbContext context) : IQueryHandler<GetOwnerQuery, OwnerDto>
{
    private readonly DbSet<Owner> _owners = context.Owner;

    public Task<OwnerDto> HandleAsync(GetOwnerQuery query, CancellationToken cancellationToken)
        => _owners
            .AsNoTracking()
            .Include(x =>  x.Users.Where(u => query.WithOnlyActiveUsers == false || u.State == State.Activate()))
            .Include(x => x.Groups)
            .ThenInclude(x => x.Users)
            .Select(x => x.AsDto())
            .FirstOrDefaultAsync(cancellationToken);
}