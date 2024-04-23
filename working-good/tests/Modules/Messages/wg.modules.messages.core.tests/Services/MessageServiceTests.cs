using NSubstitute;
using wg.modules.messages.core.Events;
using wg.modules.messages.core.Services;
using wg.modules.messages.core.Services.Abstractions;
using wg.modules.messages.core.Services.Commands;
using wg.shared.abstractions.Messaging;
using wg.tests.shared.Mocks;
using Xunit;

namespace wg.modules.messages.core.tests.Services;

public sealed class MessageServiceTests
{
    [Fact]
    public async Task CreateMessage_GivenExistingActiveEmployeeWithNullTicketNumber_ShouldSendMessageReceivedEvent()
    {
        //arrange
        var command = new CreateMessage("test@test.pl", "My test ticket",
            "My test content", null);
        
        //act
        await _messageService.CreateMessage(command);
        
        //assert
        await _messageBroker
            .Received(1)
            .PublishAsync(Arg.Is<MessageReceived>(arg
                => arg.CreatedAt == _now
                   && arg.Sender == command.Email
                   && arg.Subject == command.Subject
                   && arg.Content == command.Content
                   && arg.TicketNumber == null));
    }
    
    [Fact]
    public async Task CreateMessage_GivenExistingActiveEmployee_ShouldSendMessageReceivedEvent()
    {
        //arrange
        var command = new CreateMessage("test@test.pl", "My test ticket",
            "My test content", 1);
        
        //act
        await _messageService.CreateMessage(command);
        
        //assert
        await _messageBroker
            .Received(1)
            .PublishAsync(Arg.Is<MessageReceived>(arg
                => arg.CreatedAt == _now
                   && arg.Sender == command.Email
                   && arg.Subject == command.Subject
                   && arg.Content == command.Content
                   && arg.TicketNumber == 1));
    }

    #region MyRegion

    private readonly DateTime _now;
    private readonly IMessageBroker _messageBroker;
    private readonly IMessageService _messageService;

    public MessageServiceTests()
    {
        _messageBroker = Substitute.For<IMessageBroker>();
        _now = DateTime.Now;
        _messageService = new MessageService(_messageBroker, TestsClock.Create(_now));
    }

    #endregion
}