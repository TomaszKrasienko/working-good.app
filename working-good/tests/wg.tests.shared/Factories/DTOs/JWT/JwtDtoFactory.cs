using Bogus;
using wg.shared.abstractions.Auth.DTOs;

namespace wg.tests.shared.Factories.DTOs.JWT;

 public static class JwtDtoFactory
 {
     public static List<JwtDto> Get(int count = 1)
         => GetFaker().Generate(1);
    
    private static Faker<JwtDto> GetFaker()
        => new Faker<JwtDto>().RuleFor(u => u.Token, f => f.Lorem.Word());
}