using wg.shared.abstractions.Exceptions;

namespace wg.modules.owner.domain.Exceptions;

public sealed class UserAlreadyRegisteredException : WgException
{
    public UserAlreadyRegisteredException(Guid id)
        :   base($"User with id: {id} already registered")
    {
        
    }

    public UserAlreadyRegisteredException(string email)
        : base($"User with email: {email} already registered")
    {
        
    }
}