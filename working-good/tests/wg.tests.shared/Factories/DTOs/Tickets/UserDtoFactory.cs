using Bogus;
using wg.modules.owner.domain.ValueObjects.User;
using wg.modules.tickets.application.Clients.Owner.DTO;

namespace wg.tests.shared.Factories.DTOs.Tickets;

internal static class UserDtoFactory
{
    internal static List<UserDto> Get(int count = 1)
    {
        var userDtoFaker = new Faker<UserDto>()
            .RuleFor(f => f.Id, p => Guid.NewGuid())
            .RuleFor(f => f.Email, p => p.Internet.Email())
            .RuleFor(f => f.FirstName, p => p.Name.FirstName())
            .RuleFor(f => f.LastName, p => p.Name.LastName())
            .RuleFor(f => f.Role, p => Role.User().Value)
            .RuleFor(f => f.State, p => State.Activate().Value);

        return userDtoFaker.Generate(count);
    }
}