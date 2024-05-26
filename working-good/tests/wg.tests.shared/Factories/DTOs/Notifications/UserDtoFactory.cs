using Bogus;
using wg.modules.notifications.core.Clients.Owner.DTO;
using wg.modules.owner.domain.ValueObjects.User;

namespace wg.tests.shared.Factories.DTOs.Notifications;

internal static class UserDtoFactory
{
    internal static UserDto Get()
        => Get(1).Single();
    
    internal static List<UserDto> Get(int count)
        => GetFaker().Generate(count);
    
    private static Faker<UserDto> GetFaker()
        => new Faker<UserDto>()
            .RuleFor(f => f.Id, v => Guid.NewGuid())
            .RuleFor(f => f.Email, v => v.Internet.Email())
            .RuleFor(f => f.FirstName, v => v.Name.FirstName())
            .RuleFor(f => f.LastName, v => v.Name.LastName())
            .RuleFor(f => f.Role, v => v.PickRandom(Role.AvailableRoles))
            .RuleFor(f => f.State, v => State.Activate().Value);
}