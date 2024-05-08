using Bogus;
using wg.modules.owner.domain.Entities;

namespace wg.tests.shared.Factories.Owners;

internal static class UserFactory
{
    internal static User GetInOwner(Owner owner, string role)
        => GetInOwner(owner, role, 1).Single();

    internal static IEnumerable<User> GetInOwner(Owner owner, string role, int count)
    {
        var users = GetFaker(role).Generate(count);
        foreach (var user in users)
        {
            owner.AddUser(user.Id, user.Email, user.FullName.FirstName,
                user.FullName.LastName, user.Password, user.Role);
        }

        return owner.Users;
    }
    
    private static Faker<User> GetFaker(string role)
        => new Faker<User>()
            .CustomInstantiator(f => User.Create(
            Guid.NewGuid(),
            f.Internet.Email(),
            f.Name.FirstName(),
            f.Name.LastName(),
            f.Lorem.Word(),
            role));
}