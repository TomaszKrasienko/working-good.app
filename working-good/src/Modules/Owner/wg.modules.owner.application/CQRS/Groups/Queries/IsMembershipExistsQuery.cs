using wg.modules.owner.application.DTOs;
using wg.shared.abstractions.CQRS.Queries;

namespace wg.modules.owner.application.CQRS.Groups.Queries;

public record IsMembershipExistsQuery(Guid UserId, Guid GroupId) : IQuery<IsExistsDto>;