using wg.modules.owner.application.DTOs;
using wg.shared.abstractions.CQRS.Queries;

namespace wg.modules.owner.application.CQRS.Groups.Queries;

public sealed record IsUserInGroupQuery(Guid UserId, Guid GroupId) : IQuery<IsUserInGroupDto>;