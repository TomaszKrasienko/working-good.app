using System.Collections.Immutable;
using Bogus;
using wg.modules.tickets.application.Clients.Owner.DTO;

namespace wg.tests.shared.Factories.DTOs.Tickets.Owner;

internal static class OwnerDtoFactory
{
    internal static OwnerDto Get()
        => GetFaker().Generate(1).Single();

    internal static OwnerDto GetWithUsers(int userCount = 1)
    {
        var ownerDto = GetFaker().Generate(1).Single();
        ownerDto.Users = UserDtoFactory.Get(userCount).ToImmutableList();
        return ownerDto;
    }

    private static Faker<OwnerDto> GetFaker(int userCount = 0)
        => new Faker<OwnerDto>()
            .RuleFor(f => f.Id, v => Guid.NewGuid())
            .RuleFor(f => f.Name, v => v.Lorem.Word());
}