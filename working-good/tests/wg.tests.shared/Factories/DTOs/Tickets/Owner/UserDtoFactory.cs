using Bogus;
using wg.modules.owner.domain.ValueObjects.User;
using wg.modules.tickets.application.Clients.Owner.DTO;

namespace wg.tests.shared.Factories.DTOs.Tickets.Owner;

internal static class UserDtoFactory
{
    internal static UserDto Get()
        => Get(1).Single();
    
    internal static List<UserDto> Get(int count)
        => GetFaker().Generate(count);
    
    private static Faker<UserDto> GetFaker()
        => new Faker<UserDto>()
            .RuleFor(f => f.Id, p => Guid.NewGuid())
            .RuleFor(f => f.Email, p => p.Internet.Email())
            .RuleFor(f => f.FirstName, p => p.Name.FirstName())
            .RuleFor(f => f.LastName, p => p.Name.LastName())
            .RuleFor(f => f.Role, p => Role.User().Value)
            .RuleFor(f => f.State, p => State.Activate().Value);
    
}