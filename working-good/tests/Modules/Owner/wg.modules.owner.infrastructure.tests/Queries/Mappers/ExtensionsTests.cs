using Shouldly;
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
}