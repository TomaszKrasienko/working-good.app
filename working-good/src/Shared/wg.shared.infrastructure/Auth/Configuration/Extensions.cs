using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using wg.shared.abstractions.Auth;
using wg.shared.infrastructure.Auth.Configuration.Models;
using wg.shared.infrastructure.Configuration;

namespace wg.shared.infrastructure.Auth.Configuration;

internal static class Extensions
{
    private const string SectionName = "Jwt";
    
    internal static IServiceCollection AddAuth(this IServiceCollection services, IConfiguration configuration)
        => services
            .AddOptions(configuration)
            .AddServices()
            .AddAuthConfiguration(configuration);

    private static IServiceCollection AddOptions(this IServiceCollection services, IConfiguration configuration)
        => services
            .Configure<JwtOptions>(configuration.GetSection(SectionName));

    private static IServiceCollection AddServices(this IServiceCollection services)
        => services
            .AddSingleton<IAuthenticator, JwtAuthenticator>();

    private static IServiceCollection AddAuthConfiguration(this IServiceCollection services, IConfiguration configuration)
    {
        var options = services.GetOptions<JwtOptions>();
        var tokenValidationParameters = new TokenValidationParameters()
        {
            RequireAudience = true,
            ValidIssuer = options.Issuer,
            ValidAudience = options.Audience,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            RequireExpirationTime = true,
            ClockSkew = TimeSpan.Zero
        };

        if (string.IsNullOrWhiteSpace(options.SigningKey))
        {
            throw new ArgumentException("Missing issuer signing key");
        }

        var rawKey = Encoding.UTF8.GetBytes(options.SigningKey);
        tokenValidationParameters.IssuerSigningKey = new SymmetricSecurityKey(rawKey);
        services.AddAuthentication(o =>
            {
                o.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                o.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(o =>
            {
                o.TokenValidationParameters = tokenValidationParameters;
            });

        return services;
    }

    internal static WebApplication UseAuth(this WebApplication app)
    {
        app.UseAuthentication();
        app.UseAuthorization();
        return app;
    }
}