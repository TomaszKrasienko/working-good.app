using wg.modules.owner.domain.ValueObjects.User;
using wg.shared.abstractions.Exceptions;

namespace wg.modules.owner.domain.Exceptions;

public sealed class InvalidFirstUserRoleException(Guid id) 
    : WgException($"First user can not have other role than {Role.Manager()}");