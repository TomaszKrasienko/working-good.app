using Bogus;
using wg.modules.owner.domain.Entities;

namespace wg.tests.shared.Factories.Owners;

internal static class GroupFactory
{
    internal static Group GetInOwner(Owner owner)
        => GetInOwner(owner, 1).Single();

    internal static IEnumerable<Group> GetInOwner(Owner owner, int count)
    {
        var groups = GetFaker().Generate(count);
        foreach (var group in groups)
        {
            owner.AddGroup(group.Id, group.Title);   
        }
        return owner.Groups; 
    }
    
    private static Faker<Group> GetFaker()
        => new Faker<Group>()
            .CustomInstantiator(f => Group.Create(
                Guid.NewGuid(),
                f.Lorem.Word()));
}