using Bogus;
using wg.modules.tickets.application.Clients.Owner.DTO;

namespace wg.tests.shared.Factories.DTOs.Tickets.Owner;

internal static class OwnerDtoFactory
{
    internal static OwnerDto Get()
        => GetFaker().Generate(1).Single();
    
    private static Faker<OwnerDto> GetFaker()
        => new Faker<OwnerDto>()
            .RuleFor(f => f.Id, v => Guid.NewGuid())
            .RuleFor(f => f.Name, v => v.Lorem.Word());
}