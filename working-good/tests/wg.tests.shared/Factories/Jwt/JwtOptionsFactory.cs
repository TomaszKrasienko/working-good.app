using Bogus;
using wg.shared.infrastructure.Auth.Configuration.Models;

namespace wg.tests.shared.Factories.Jwt;

internal static class JwtOptionsFactory
{
    public static JwtOptions Get()
        => GetFaker().Generate(1).Single();
    
    private static Faker<JwtOptions> GetFaker()
        => new Faker<JwtOptions>()
            .RuleFor(f => f.Audience, v => v.Lorem.Word())
            .RuleFor(f => f.Issuer, v => v.Lorem.Word())
            .RuleFor(f => f.SigningKey, v => "11DF469417BB6C884189FEAAAFB6A8AB38983A7C34582E77BD816A7AC5")
            .RuleFor(f => f.Expiry, v => TimeSpan.FromHours(2));
}