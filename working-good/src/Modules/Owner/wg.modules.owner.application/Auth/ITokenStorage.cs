using wg.shared.abstractions.Auth.DTOs;

namespace wg.modules.owner.application.Auth;

public interface ITokenStorage
{
    void Set(JwtDto dto);
    JwtDto Get();
}