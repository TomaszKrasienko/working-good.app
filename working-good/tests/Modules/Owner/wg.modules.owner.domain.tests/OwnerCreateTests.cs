using Shouldly;
using wg.modules.owner.domain.Entities;
using wg.modules.owner.domain.Exceptions;
using wg.modules.owner.domain.ValueObjects.User;
using wg.shared.abstractions.Kernel.Exceptions;
using Xunit;

namespace wg.modules.owner.domain.tests;

public sealed class OwnerCreateTests
{    
    [Fact]
    public void Create_ForValidArguments_ShouldReturnOwnerWithFilledFields()
    {
        //arrange
        var name = "company_name";
        
        //act
        var result = Owner.Create(Guid.NewGuid(), name);
        
        //assert
        result.ShouldNotBeNull();
        result.Name.Value.ShouldBe(name);
    }
    
    [Fact]
    public void Create_GivenEmptyName_ShouldThrowEmptyNameException()
    {
        //act
        var exception = Record.Exception(() => Owner.Create(Guid.NewGuid(),string.Empty));
        
        //assert
        exception.ShouldBeOfType<EmptyNameException>();
    }
}