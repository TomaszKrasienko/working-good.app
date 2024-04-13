using wg.modules.owner.application.DTOs;
using wg.shared.abstractions.CQRS.Queries;

namespace wg.modules.owner.application.CQRS.Owners.Queries;

public sealed record GetOwnerQuery(bool WithOnlyActiveUsers = false) : IQuery<OwnerDto>;