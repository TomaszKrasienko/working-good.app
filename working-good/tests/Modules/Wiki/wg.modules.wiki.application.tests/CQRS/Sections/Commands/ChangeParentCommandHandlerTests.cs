using NSubstitute;
using Shouldly;
using wg.modules.wiki.application.CQRS.Sections.Commands.ChangeParent;
using wg.modules.wiki.application.Exceptions;
using wg.modules.wiki.core.Exceptions;
using wg.modules.wiki.domain.Repositories;
using wg.shared.abstractions.CQRS.Commands;
using wg.tests.shared.Factories.Wiki;
using Xunit;

namespace wg.modules.wiki.application.tests.CQRS.Sections.Commands;

public sealed class ChangeParentCommandHandlerTests
{
    private Task Act(ChangeParentCommand command) => _handler.HandleAsync(command, default);
    
    [Fact]
    public async Task ChangeParentAsync_GivenExistingSectionAndParentSection_ShouldAddParentSectionToSectionAndUpateSection()
    {
         //arrange
         var section = SectionsFactory.Get();
         var parentSection = SectionsFactory.Get();
         var command = new ChangeParentCommand(section.Id, Guid.NewGuid());

         _sectionRepository
             .GetByIdAsync(command.SectionId, default)
             .Returns(section);

         _sectionRepository
             .GetByIdAsync(command.ParentSectionId!.Value, default)
             .Returns(parentSection);
         
         //act
         await Act(command);
         
         //assert
         await _sectionRepository
             .Received(1)
             .UpdateAsync(section, default);
         
         section.Parent.ShouldBe(parentSection);
    }

    [Fact]
    public async Task ChangeParentAsync_GivenNullParentSectionId_ShouldChangeParentToNullAndUpdate()
    {
     //arrange
     var section = SectionsFactory.Get();
     var parentSection = SectionsFactory.Get();
     section.ChangeParent(parentSection);
     var command = new ChangeParentCommand(section.Id, null);

     _sectionRepository
         .GetByIdAsync(command.SectionId, default)
         .Returns(section);
     
     //act
     await Act(command);
     
     //assert
     await _sectionRepository
         .Received(1)
         .UpdateAsync(section, default);

     section.Parent.ShouldBeNull();
    }
     
     [Fact]
     public async Task HandleAsync_GivenNotExistingParentId_ShouldThrowParentSectionNotFoundException()
     {
         //arrange
         var section = SectionsFactory.Get();
         var command = new ChangeParentCommand(section.Id, Guid.NewGuid());

         _sectionRepository
             .GetByIdAsync(command.SectionId, default)
             .Returns(section);

         await _sectionRepository
             .GetByIdAsync(command.ParentSectionId!.Value, default);
         
         //act
         var exception = await Record.ExceptionAsync(async () => await Act(command));
         
         //assert
         exception.ShouldBeOfType<ParentSectionNotFoundException>();
     }

     [Fact]
     public async Task HandleAsync_GivenNotExistingSectionId_ShouldThrowSectionNotFoundException()
     {
         //arrange
         var command = new ChangeParentCommand(Guid.NewGuid(), Guid.NewGuid());

         await _sectionRepository
             .GetByIdAsync(command.SectionId, default);
         
         //act
         var exception = await Record.ExceptionAsync(async () => await Act(command));
         
         //assert
         exception.ShouldBeOfType<SectionNotFoundException>();
     }
    
    #region arrange
    private readonly ISectionRepository _sectionRepository;
    private readonly ICommandHandler<ChangeParentCommand> _handler;

    public ChangeParentCommandHandlerTests()
    {
        _sectionRepository = Substitute.For<ISectionRepository>();
        _handler = new ChangeParentCommandHandler(_sectionRepository);
    }
    #endregion
}