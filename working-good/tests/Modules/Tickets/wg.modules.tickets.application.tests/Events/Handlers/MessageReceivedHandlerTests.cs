using NSubstitute;
using wg.modules.tickets.application.Events.External;
using wg.modules.tickets.application.Events.External.Handlers;
using wg.modules.tickets.domain.Services;
using wg.shared.abstractions.Events;
using wg.tests.shared.Factories.Events;
using Xunit;

namespace wg.modules.tickets.application.tests.Events.Handlers;

public sealed class MessageReceivedHandlerTests
{
    private Task Act(MessageReceived @event) => _handler.HandleAsync(@event);

    [Fact]
    public async Task HandleAsync_GivenEvent_ShouldPassToNewMessageDomainService()
    {
        //arrange
        var @event = MessageReceivedFactory.Get();
        
        //act
        await _handler.HandleAsync(@event);
        
        //assert
        await _newMessageDomainService
            .Received(1)
            .AddNewMessage(Arg.Any<Guid>(), @event.Sender, @event.Subject, @event.Content, @event.CreatedAt,
                @event.TicketNumber, null, null);
    }
    
    #region arrange
    private readonly INewMessageDomainService _newMessageDomainService;
    private readonly IEventHandler<MessageReceived> _handler;

    public MessageReceivedHandlerTests()
    {
        _newMessageDomainService = Substitute.For<INewMessageDomainService>();
        _handler = new MessageReceivedHandler(_newMessageDomainService);
    }
    #endregion
}