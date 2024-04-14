using Bogus;
using wg.modules.tickets.application.Clients.Owner.DTO;

namespace wg.tests.shared.Factories.DTOs.Tickets.Owner;

internal static class GroupDtoFactory
{
    internal static List<GroupDto> Get(int count = 1, Guid? id = null)
        => GetFaker(id).Generate(count);
    
    private static Faker<GroupDto> GetFaker(Guid? id = null)
        => new Faker<GroupDto>()
            .RuleFor(f => f.Id, v => id ?? Guid.NewGuid())
            .RuleFor(f => f.Title, v => v.Lorem.Sentence(null, 7));

}