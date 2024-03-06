using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using wg.modules.owner.application.Auth;
using wg.modules.owner.domain.Entities;

namespace wg.modules.owner.infrastructure.Auth.Configuration;

internal static class Extensions
{
    internal static IServiceCollection AddAuth(this IServiceCollection services)
        => services
            .AddSingleton<IPasswordHasher<User>, PasswordHasher<User>>()
            .AddSingleton<IPasswordManager, PasswordManager>()
            .AddScoped<ITokenStorage, HttpContextTokenStorage>()
            .AddHttpContextAccessor();

}