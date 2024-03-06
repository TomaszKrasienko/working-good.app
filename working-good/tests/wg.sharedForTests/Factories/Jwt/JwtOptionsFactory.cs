using Bogus;
using wg.shared.infrastructure.Auth.Configuration.Models;

namespace wg.sharedForTests.Factories.Jwt;

internal static class JwtOptionsFactory
{
    public static JwtOptions Get()
    {
        var faker = new Faker<JwtOptions>()
            .RuleFor(f => f.Audience, v => v.Lorem.Word())
            .RuleFor(f => f.Issuer, v => v.Lorem.Word())
            .RuleFor(f => f.SigningKey, v => "11DF469417BB6C884189FEAAAFB6A8AB38983A7C34582E77BD816A7AC5")
            .RuleFor(f => f.Expiry, v => TimeSpan.FromHours(2));
        return faker.Generate(1).Single();
    }
}