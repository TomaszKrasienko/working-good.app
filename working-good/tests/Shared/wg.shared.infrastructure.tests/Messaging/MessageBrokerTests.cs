using Microsoft.AspNetCore.Http.Features;
using NSubstitute;
using wg.shared.abstractions.Events;
using wg.shared.abstractions.Messaging;
using wg.shared.infrastructure.Messaging;
using wg.tests.shared.Factories.Events;
using Xunit;

namespace wg.shared.infrastructure.tests.Messaging;

public sealed class MessageBrokerTests
{
    [Fact]
    public async Task PublishAsync_ForNotEmptyMessages_ShouldSendByAsyncMessageDispatcher()
    {
        //arrange
        var messages = TestEventsFactory.Get();
        
        //act
        await _messageBroker.PublishAsync(messages);
        
        //assert
        await _asyncMessageDispatcher
            .Received(messages.Length)
            .PublishAsync(Arg.Any<IEvent>());
    }
    
    #region arrange
    private readonly IAsyncMessageDispatcher _asyncMessageDispatcher;
    private readonly MessageBroker _messageBroker;

    public MessageBrokerTests()
    {
        _asyncMessageDispatcher = Substitute.For<IAsyncMessageDispatcher>();
        _messageBroker = new MessageBroker(_asyncMessageDispatcher);
    }
    #endregion
}