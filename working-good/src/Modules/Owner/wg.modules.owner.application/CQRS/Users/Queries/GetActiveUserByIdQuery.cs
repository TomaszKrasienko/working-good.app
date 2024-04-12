using wg.modules.owner.application.DTOs;
using wg.shared.abstractions.CQRS.Queries;

namespace wg.modules.owner.application.CQRS.Users.Queries;

public sealed record GetActiveUserByIdQuery(Guid Id) : IQuery<UserDto>;