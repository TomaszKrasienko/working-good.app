using Bogus;
using wg.modules.owner.domain.Entities;

namespace wg.tests.shared.Factories.Owners;

public static class GroupFactory
{
    public static Group GetGroupInOwner(Owner owner)
    {
        var groupFaker = new Faker<Group>()
            .CustomInstantiator(f => Group.Create(
                Guid.NewGuid(),
                f.Lorem.Word()));

        var group = groupFaker.Generate(1).Single();
        owner.AddGroup(group.Id, group.Title);
        return owner.Groups.Single();
    }
}