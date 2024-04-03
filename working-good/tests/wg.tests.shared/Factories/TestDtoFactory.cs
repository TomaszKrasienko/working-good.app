using Bogus;
using wg.tests.shared.Models;

namespace wg.tests.shared.Factories;

public static class TestDtoFactory
{
    public static List<TestDto> Get(int count = 1)
    {
        var dtoFaker = new Faker<TestDto>()
            .RuleFor(f => f.Value, p => p.Lorem.Word());
        return dtoFaker.Generate(count);
    }
}