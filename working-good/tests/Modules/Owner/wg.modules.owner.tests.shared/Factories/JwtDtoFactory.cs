using Bogus;
using wg.shared.abstractions.Auth.DTOs;

namespace wg.modules.owner.tests.shared.Factories;

 public static class JwtDtoFactory
{
    public static JwtDto Get()
    {
        var tokenFaker = new Faker<JwtDto>().RuleFor(u => u.Token, f => f.Lorem.Word());
        return tokenFaker.Generate(1).Single();
    }
}