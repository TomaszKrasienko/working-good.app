using Shouldly;
using wg.tests.shared.Factories.Wiki;
using Xunit;

namespace wg.modules.wiki.core.tests.Entities;

public sealed class SectionTests
{
    [Fact]
    public void ChangeParent_GivenParent_ShouldChangeParent()
    {
        //arrange
        var section = SectionsFactory.Get();
        var parentSection = SectionsFactory.Get();
        
        //act
        section.ChangeParent(parentSection);
        
        //assert
        section.Parent.ShouldBe(parentSection);
    }
}