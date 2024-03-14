using Shouldly;
using wg.modules.owner.domain.Entities;
using wg.shared.abstractions.Kernel.Exceptions;
using Xunit;

namespace wg.modules.owner.domain.tests;

public sealed class GroupCreateTests
{
    [Fact]
    public void Create_GivenForValidArguments_ShouldReturnGroupWithFilledFields()
    {
        //arrange
        var id = Guid.NewGuid();
        var title = "Group title";
        
        //act
        var result = Group.Create(id, title);
        
        //assert
        result.Id.Value.ShouldBe(id);
        result.Title.Value.ShouldBe(title);
    }

    [Fact]
    public void Create_GivenEmptyTitle_ShouldThrow()
    {
        //act
        var exception = Record.Exception(() => Group.Create(Guid.NewGuid(), string.Empty));
        
        //assert
        exception.ShouldBeOfType<EmptyTitleException>();
    }
}