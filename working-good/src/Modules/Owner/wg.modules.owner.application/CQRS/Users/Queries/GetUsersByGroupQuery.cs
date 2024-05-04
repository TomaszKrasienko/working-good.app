using System.Collections.Immutable;
using wg.modules.owner.application.DTOs;
using wg.shared.abstractions.CQRS.Queries;

namespace wg.modules.owner.application.CQRS.Users.Queries;

public sealed record GetUsersByGroupQuery(Guid GroupId) : IQuery<IEnumerable<UserDto>>;