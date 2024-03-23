using Microsoft.AspNetCore.Http;
using wg.shared.abstractions.Context;

namespace wg.shared.infrastructure.Context.Factories;

internal sealed class IdentityContextFactory(
    IHttpContextAccessor contextAccessor) : IIdentityContextFactory
{
    public IIdentityContext Create()
        => new IdentityContext(contextAccessor.HttpContext);
}