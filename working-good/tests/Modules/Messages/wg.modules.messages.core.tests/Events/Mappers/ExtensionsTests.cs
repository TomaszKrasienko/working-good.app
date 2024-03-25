using Shouldly;
using wg.modules.messages.core.Events.Mappers;
using Xunit;
using wg.tests.shared.Factories.Messages;

namespace wg.modules.messages.core.tests.Events.Mappers;

public sealed class ExtensionsTests
{
    [Fact]
    public void AsEvent_GivenClientMessageWithNumber_ShouldReturnEventWithNumber()
    {
        //arrange
        var clientMessage = ClientMessageFactory.Get(true);
        
        //act
        var result = clientMessage.AsEvent();
        
        //assert
        result.ShouldNotBeNull();
        result.Subject.ShouldBe(clientMessage.Subject);
        result.Content.ShouldBe(clientMessage.Content);
        result.CreatedAt.ShouldBe(clientMessage.CreatedAt);
        result.Sender.ShouldBe(clientMessage.Sender);
        result.TicketNumber.ShouldBe(clientMessage.Number);
    }

    [Fact]
    public void AsEvent_GivenClientMessageWithoutNumber_ShouldReturnEventWithNumber()
    {
        //arrange
        var clientMessage = ClientMessageFactory.Get(false);
        
        //act
        var result = clientMessage.AsEvent();
        
        //assert
        result.ShouldNotBeNull();
        result.Subject.ShouldBe(clientMessage.Subject);
        result.Content.ShouldBe(clientMessage.Content);
        result.CreatedAt.ShouldBe(clientMessage.CreatedAt);
        result.Sender.ShouldBe(clientMessage.Sender);
        result.TicketNumber.ShouldBeNull();
    }
}