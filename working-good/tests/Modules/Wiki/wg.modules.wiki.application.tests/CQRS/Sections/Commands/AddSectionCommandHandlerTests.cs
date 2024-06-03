using NSubstitute;
using Shouldly;
using wg.modules.wiki.application.CQRS.Sections.Commands;
using wg.modules.wiki.application.Exceptions;
using wg.modules.wiki.core.Exceptions;
using wg.modules.wiki.domain.Entities;
using wg.modules.wiki.domain.Repositories;
using wg.shared.abstractions.CQRS.Commands;
using wg.tests.shared.Factories.Wiki;
using Xunit;

namespace wg.modules.wiki.application.tests.CQRS.Sections.Commands;

public sealed class AddSectionCommandHandlerTests
{
    private Task Act(AddSectionCommand command) => _handler.HandleAsync(command, default);

     [Fact]
    public async Task HandleAsync_GivenCommandWithParentId_ShouldAddSectionByRepository()
    {
         //arrange
         var parent = SectionsFactory.Get();
         var command = new AddSectionCommand(Guid.NewGuid(), "Section name", parent.Id);

         _sectionRepository
             .GetByIdAsync(command.ParentId!.Value, default)
             .Returns(parent);

         _sectionRepository
             .IsNameExistsAsync(command.Name, default)
             .Returns(false);
         
         //act
         await Act(command);
         
         //assert
         await _sectionRepository
             .Received(1)
             .AddAsync(Arg.Is<Section>(arg
                 => arg.Id.Equals(command.Id)
                 && arg.Name == command.Name
                 && arg.Parent == parent), default);
    }
    
    [Fact]
    public async Task HandleAsync_GivenCommandWithout_ShouldAddSectionByRepository()
    {
         //arrange
         var command = new AddSectionCommand(Guid.NewGuid(), "Section name", null);
         
         _sectionRepository
             .IsNameExistsAsync(command.Name, default)
             .Returns(false);
         
         //act
         await Act(command);
         
         //assert
         await _sectionRepository
             .Received(1)
             .AddAsync(Arg.Is<Section>(arg
                 => arg.Id.Equals(command.Id)
                    && arg.Name == command.Name
                    && arg.Parent == null), default);

         await _sectionRepository
             .Received(0)
             .GetByIdAsync(Arg.Any<Guid>(), default);
    }
    
    [Fact]
    public async Task HandleAsync_GivenAlreadyRegisteredName_ShouldThrowSectionNameAlreadyRegisteredException()
    {
         //arrange
         var command = new AddSectionCommand(Guid.NewGuid(), "Section name", null);
         
         _sectionRepository
             .IsNameExistsAsync(command.Name, default)
             .Returns(true);
             
         //act
         var exception = await Record.ExceptionAsync(async () => await Act(command));
         
         //assert
         exception.ShouldBeOfType<SectionNameAlreadyRegisteredException>();
    }
    
    [Fact]
    public async Task HandleAsync_GivenCommandWithNotExistingParentId_ShouldThrowParentSectionNotFoundException()
    {
        //arrange
         var command = new AddSectionCommand(Guid.NewGuid(), "Section name", Guid.NewGuid());
         
         _sectionRepository
             .IsNameExistsAsync(command.Name, default)
             .Returns(false);
         //act
         var exception = await Record.ExceptionAsync(async () => await Act(command));
         
         //assert
         exception.ShouldBeOfType<ParentSectionNotFoundException>();
    }
    
    #region arrange
    private readonly ISectionRepository _sectionRepository;
    private readonly ICommandHandler<AddSectionCommand> _handler;

    public AddSectionCommandHandlerTests()
    {
        _sectionRepository = Substitute.For<ISectionRepository>();
        _handler = new AddSectionCommandHandler(_sectionRepository);
    }
    #endregion
}