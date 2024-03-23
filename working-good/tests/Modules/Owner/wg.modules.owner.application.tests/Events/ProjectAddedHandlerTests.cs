using NSubstitute;
using Shouldly;
using wg.modules.owner.application.Events.External;
using wg.modules.owner.application.Events.External.Handlers;
using wg.modules.owner.application.Exceptions;
using wg.modules.owner.domain.Repositories;
using wg.tests.shared.Factories.Owners;
using Xunit;

namespace wg.modules.owner.application.tests.Events;

public sealed class ProjectAddedHandlerTests
{
    private Task Act(ProjectAdded @event) => _handler.HandleAsync(@event);
    
    [Fact]
    public async Task Handle_GivenExistingOwner_ShouldUpdateOwnerByRepository()
    {
        //arrange
        var owner = OwnerFactory.Get();
        _ownerRepository
            .GetAsync()
            .Returns(owner);
        var @event = new ProjectAdded(Guid.NewGuid(), "Group");
        
        //act
        await Act(@event);
        
        //assert
        await _ownerRepository
            .Received(1)
            .UpdateAsync(owner);
    }
    
    [Fact]
    public async Task Handle_GivenNotExistingOwner_ShouldOwnerNotFoundException()
    {
        //arrange
        var @event = new ProjectAdded(Guid.NewGuid(), "Group");
        
        //act
        var exception = await Record.ExceptionAsync(async () => await Act(@event));
        
        //assert
        exception.ShouldBeOfType<OwnerNotFoundException>();
    }
    
    #region arrange
    private readonly IOwnerRepository _ownerRepository;
    private readonly ProjectAddedHandler _handler;

    public ProjectAddedHandlerTests()
    {
        _ownerRepository = Substitute.For<IOwnerRepository>();
        _handler = new ProjectAddedHandler(_ownerRepository);
    }
    #endregion
}