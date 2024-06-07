using NSubstitute;
using wg.modules.wiki.application.Events.External;
using wg.modules.wiki.application.Events.External.Handler;
using wg.modules.wiki.domain.Entities;
using wg.modules.wiki.domain.Repositories;
using wg.shared.abstractions.Events;
using Xunit;

namespace wg.modules.wiki.application.tests.Events.External;

public sealed class CompanyAddedHandlerTests
{
    private Task Act(CompanyAdded @event) => _handler.HandleAsync(@event);

    [Fact]
    public async Task HandleAsync_GivenNotExistingName_ShouldAddSectionByRepository()
    {
        //arrange
        var @event = new CompanyAdded(Guid.NewGuid(), "Section name");
        _sectionRepository
            .IsNameExistsAsync(@event.Name, default)
            .Returns(false);
        
        //act
        await Act(@event);
        
        //assert
        await _sectionRepository
            .Received(1)
            .AddAsync(Arg.Is<Section>(arg
                => arg.Name == @event.Name), default);
    }
    
    [Fact]
    public async Task HandleAsync_GivenAlreadyExistingName_ShouldNotCreateSection()
    {
        //arrange
        var @event = new CompanyAdded(Guid.NewGuid(), "Section name");
        _sectionRepository
            .IsNameExistsAsync(@event.Name, default)
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
    private readonly IEventHandler<CompanyAdded> _handler;
    
    public CompanyAddedHandlerTests()
    {
        _sectionRepository = Substitute.For<ISectionRepository>();
        _handler = new CompanyAddedHandler(_sectionRepository);
    }
    #endregion
}