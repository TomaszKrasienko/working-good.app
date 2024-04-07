using NSubstitute;
using Shouldly;
using wg.modules.owner.application.Events.External;
using wg.modules.owner.application.Events.External.Handlers;
using wg.modules.owner.application.Exceptions;
using wg.modules.owner.domain.Repositories;
using wg.tests.shared.Factories.Owners;
using Xunit;

namespace wg.modules.owner.application.tests.Events;

public sealed class ProjectEditedHandlerTests
{
    private Task Act(ProjectEdited @event) => _handler.HandleAsync(@event);

    [Fact]
    public async Task HandleAsync_ForExistingOwner_ShouldUpdateOwnerByRepository()
    {
        //arrange
        var owner = OwnerFactory.Get();
        var group = GroupFactory.GetGroupInOwner(owner);
        var @event = new ProjectEdited(group.Id, "NewProjectTitle");
        _ownerRepository
            .GetAsync()
            .Returns(owner);
        
        //act
        await Act(@event);
        
        //assert
        var updatedGroup = owner.Groups.FirstOrDefault(x => x.Id.Equals(@event.Id));
        updatedGroup!.Title.Value.ShouldBe(@event.Title);
        
        await _ownerRepository
            .Received(1)
            .UpdateAsync(owner);
    }
    
    [Fact]
    public async Task HandleAsync_ForNoExistingOwner_ShouldThrowOwnerNotFoundException()
    {
        //arrange
        var @event = new ProjectEdited(Guid.NewGuid(), "Test");
        
        //assert
        var exception = await Record.ExceptionAsync(async () => await Act(@event));
        
        //act
        exception.ShouldBeOfType<OwnerNotFoundException>();
    }
    
    #region arrange

    private readonly IOwnerRepository _ownerRepository;
    private readonly ProjectEditedHandler _handler;

    public ProjectEditedHandlerTests()
    {
        _ownerRepository = Substitute.For<IOwnerRepository>();
        _handler = new ProjectEditedHandler(_ownerRepository);
    }

    #endregion
}