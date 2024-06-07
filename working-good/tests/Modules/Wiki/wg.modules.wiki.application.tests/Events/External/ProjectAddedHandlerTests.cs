using NSubstitute;
using wg.modules.companies.domain.Entities;
using wg.modules.wiki.application.Events.External;
using wg.modules.wiki.application.Events.External.Handler;
using wg.modules.wiki.domain.Entities;
using wg.modules.wiki.domain.Repositories;
using wg.shared.abstractions.Events;
using Xunit;

namespace wg.modules.wiki.application.tests.Events.External;

public sealed class ProjectAddedHandlerTests
{
    private Task Act(ProjectAdded @event) => _handler.HandleAsync(@event);

    [Fact]
    public async Task HandleAsync_GivenNotExistingName_ShouldAddSection()
    {
        //arrange
        var @event = new ProjectAdded(Guid.NewGuid(), "Project name");
        _sectionRepository
            .IsNameExistsAsync(@event.Title, default)
            .Returns(false);
        
        //act
        await Act(@event);
        
        //assert
        await _sectionRepository
            .Received(1)
            .AddAsync(Arg.Is<Section>(arg
                => arg.Name == @event.Title), default);
    }
    
    [Fact]
    public async Task HandleAsync_GivenAlreadyExistingName_ShouldNotAddSection()
    {
        //arrange
        var @event = new ProjectAdded(Guid.NewGuid(), "Project name");
        _sectionRepository
            .IsNameExistsAsync(@event.Title, default)
            .Returns(true);
        
        //act
        await Act(@event);
        
        //assert
        await _sectionRepository
            .Received(0)
            .AddAsync(Arg.Any<Section>(), default);
    }
    
    #region arrange
    private readonly ISectionRepository _sectionRepository;
    private readonly IEventHandler<ProjectAdded> _handler;

    public ProjectAddedHandlerTests()
    {
        _sectionRepository = Substitute.For<ISectionRepository>();
        _handler = new ProjectAddedHandler(_sectionRepository);
    }
    #endregion
}