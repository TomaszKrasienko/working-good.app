using wg.shared.abstractions.Auth.DTOs;

namespace wg.shared.abstractions.Auth;

public interface IAuthenticator
{
    JwtDto CreateToken(string userId, string role);
}