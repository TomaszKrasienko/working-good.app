using Shouldly;
using wg.modules.owner.domain.Entities;
using wg.modules.owner.domain.ValueObjects.User;
using wg.modules.owner.infrastructure.Queries.Mappers;
using wg.tests.shared.Factories.Owners;
using Xunit;

namespace wg.modules.owner.infrastructure.tests.Queries.Mappers;

public sealed class ExtensionsTests
{
    [Fact]
    public void AsDto_GivenOwner_ShouldReturnOwnerDto()
    {
        //arrange
        var owner = OwnerFactory.Get();
        
        //act
        var result = owner.AsDto();
        
        //assert
        result.ShouldNotBeNull();
        result.Id.ShouldBe(owner.Id.Value);
        result.Name.ShouldBe(owner.Name.Value);
    }

    [Fact]
    public void AsDto_GivenUser_ShouldReturnUserDto()
    {
        //arrange
        var owner = OwnerFactory.Get();
        var user = UserFactory.GetUserInOwner(owner, Role.Manager());
        
        //act
        var result = user.AsDto();
        
        //assert
        result.Id.ShouldBe(user.Id.Value);
        result.Email.ShouldBe(user.Email.Value);
        result.FirstName.ShouldBe(user.FullName.FirstName);
        result.LastName.ShouldBe(user.FullName.LastName);
        result.Role.ShouldBe(user.Role.Value);
        result.State.ShouldBe(user.State.Value);
    }
}