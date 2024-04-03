using wg.modules.owner.application.DTOs;
using wg.shared.abstractions.CQRS.Queries;
using wg.shared.abstractions.Pagination;

namespace wg.modules.owner.application.CQRS.Users.Queries;

public sealed record GetUsersQuery : PaginationDto, IQuery<PagedList<UserDto>>;