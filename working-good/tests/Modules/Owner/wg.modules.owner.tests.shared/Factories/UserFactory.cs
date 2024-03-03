using Bogus;
using wg.modules.owner.domain.Entities;

namespace wg.modules.owner.tests.shared.Factories;

public static class UserFactory
{
    internal static Owner GetUserInOwner(Owner owner, string role)
    {
        var userFaker = new Faker<User>()
            .CustomInstantiator(f => User.Create(
                Guid.NewGuid(),
                f.Internet.Email(),
                f.Name.FirstName(),
                f.Name.LastName(),
                f.Lorem.Word(),
                role));
        var user = userFaker.Generate(1).Single();
        owner.AddUser(user.Id, user.Email, user.FullName.FirstName,
            user.FullName.LastName, user.Password, user.Role);
        return owner;
    }
}