using Shouldly;
using wg.modules.wiki.core.Entities;
using wg.modules.wiki.core.Exceptions;
using Xunit;

namespace wg.modules.wiki.core.tests.Entities.Create;

public sealed class SectionCreateTests
{
    [Fact]
    public void Create_GivenWithParent_ShouldReturnSectionWithParent()
    {
        //arrange
        var parent = Section.Create(Guid.NewGuid(), "Parent section");
        var name = "Child section name";
        var id = Guid.NewGuid();
        
        //act
        var result = Section.Create(id, name, parent);
        
        //assert
        result.Id.Value.ShouldBe(id);
        result.Name.Value.ShouldBe(name);
        result.Parent.ShouldBe(parent);
    }
    
    [Fact]
    public void Create_GivenWithoutParent_ShouldReturnSectionWithoutParent()
    {
        //arrange
        var name = "Child section name";
        var id = Guid.NewGuid();
        
        //act
        var result = Section.Create(id, name);
        
        //assert
        result.Id.Value.ShouldBe(id);
        result.Name.Value.ShouldBe(name);
        result.Parent.ShouldBeNull();
    }
    
    [Fact]
    public void Create_GivenEmptyName_ShouldThrowEmptySectionNameException()
    {
        //act
        var exception = Record.Exception(() => Section.Create(Guid.NewGuid(), string.Empty));
        
        //assert
        exception.ShouldBeOfType<EmptySectionNameException>();
    }
}