using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using wg.shared.abstractions.Context;

namespace wg.shared.infrastructure.Context;

internal sealed class IdentityContext : IIdentityContext
{
    public bool IsAuthenticated { get; }
    public Guid UserId { get; }
    public string Role { get; }

    public IdentityContext(HttpContext httpContext)
    {
        var user = httpContext.User;
        IsAuthenticated = user.Identity?.IsAuthenticated ?? false;
        Guid.TryParse(user.Identity?.Name, out var userId);
        UserId = userId;
        Role = user.Claims.SingleOrDefault(x => x.Type == ClaimTypes.Role)?.Value;
    }
}