using Bogus;
using wg.shared.abstractions.Auth.DTOs;

namespace wg.tests.shared.Factories.DTOs.JWT;

 internal static class JwtDtoFactory
 {
     internal static JwtDto Get()
         => Get(1).Single(); 
     
     private static List<JwtDto> Get(int count)
         => GetFaker().Generate(1);
    
    private static Faker<JwtDto> GetFaker()
        => new Faker<JwtDto>().RuleFor(u => u.Token, f => f.Lorem.Word());
}