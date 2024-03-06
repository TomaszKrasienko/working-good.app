using Microsoft.AspNetCore.Http;
using wg.modules.owner.application.Auth;
using wg.modules.owner.application.DTOs;

namespace wg.modules.owner.infrastructure.Auth;

internal sealed class HttpContextTokenStorage
    (IHttpContextAccessor httpContextAccessor): ITokenStorage
{
    private const string TokenKey = "user_jwt_token";
    
    public void Set(JwtDto dto)
        => httpContextAccessor.HttpContext?.Items.TryAdd(TokenKey, dto);

    public JwtDto Get()
    {
        if (httpContextAccessor.HttpContext is null)
        {
            return null;
        }

        if (httpContextAccessor.HttpContext.Items.TryGetValue(TokenKey, out var tokenDto))
        {
            return tokenDto as JwtDto;
        }

        return null;
    }
}