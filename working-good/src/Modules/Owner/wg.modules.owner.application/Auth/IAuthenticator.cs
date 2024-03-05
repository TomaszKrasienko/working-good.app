using wg.modules.owner.application.DTOs;

namespace wg.modules.owner.application.Auth;

public interface IAuthenticator
{
    JwtDto CreateToken(string userId, string role);
}