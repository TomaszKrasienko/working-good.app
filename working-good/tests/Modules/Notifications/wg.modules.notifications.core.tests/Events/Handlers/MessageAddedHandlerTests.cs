using NSubstitute;
using wg.modules.notifications.core.Clients.Companies;
using wg.modules.notifications.core.Clients.Companies.DTO;
using wg.modules.notifications.core.Events.External;
using wg.modules.notifications.core.Events.External.Handlers;
using wg.modules.notifications.core.Models;
using wg.modules.notifications.core.Services.Abstractions;
using wg.shared.abstractions.Events;
using wg.shared.infrastructure.Notifications;
using Xunit;

namespace wg.modules.notifications.core.tests.Events.Handlers;

public sealed class MessageAddedHandlerTests
{
    private Task Act(MessageAdded @event) => _handler.HandleAsync(@event);
    
    [Fact]
    public async Task HandleAsync_GivenEventWithFilledField_ShouldPublishByEmailPublisher()
    {
        //arrange
        var @event = new MessageAdded(1, "test subject", "test content", Guid.NewGuid());

        var employeeDto = new EmployeeDto()
        {
            Email = "test@test.pl",
            Id = (Guid)@event.EmployeeId!,
            PhoneNumber = "555000111"
        };

        _companiesApiClient
            .GetActiveEmployeeByIdAsync(Arg.Is<EmployeeIdDto>(arg => arg.Id == @event.EmployeeId))
            .Returns(employeeDto);
        
        //act
        await Act(@event);
        
        //assert
        await _emailPublisher
            .Received(1)
            .PublishAsync(Arg.Is<EmailNotification>(arg
                => arg.Subject == NotificationsDirectory.GetTicketSubject(@event.TicketNumber, @event.Content) 
                && arg.Content == @event.Content
                && arg.Recipient[0] == employeeDto.Email), default);
    }

    [Fact]
    public async Task HandleAsync_GivenNullEmployeeId_ShouldNotPublishByEmailPublisher()
    {
        //arrange
        var @event = new MessageAdded(1, "test subject", "test content", null);
        
        //act
        await Act(@event);
            
        //assert
        await _emailPublisher
            .Received(0)
            .PublishAsync(Arg.Any<EmailNotification>(), default);
    }
    
    [Fact]
    public async Task HandleAsync_GivenEmptySubject_ShouldNotPublishByEmailPublisher()
    {
        //arrange
        var @event = new MessageAdded(1, string.Empty, "test content", Guid.NewGuid());
        
        //act
        await Act(@event);
        
        //assert
        await _emailPublisher
            .Received(0)
            .PublishAsync(Arg.Any<EmailNotification>(), default);
    }
    
    [Fact]
    public async Task HandleAsync_GivenEmptyContent_ShouldNotPublishByEmailPublisher()
    {
        //arrange
        var @event = new MessageAdded(1, "test content", string.Empty, Guid.NewGuid());
        
        //act
        await Act(@event);
        
        //assert
        await _emailPublisher
            .Received(0)
            .PublishAsync(Arg.Any<EmailNotification>(), default);
    }
    
    #region arrange

    private readonly ICompaniesApiClient _companiesApiClient;
    private readonly IEmailPublisher _emailPublisher;
    private readonly IEventHandler<MessageAdded> _handler;

    public MessageAddedHandlerTests()
    {
        _companiesApiClient = Substitute.For<ICompaniesApiClient>();
        _emailPublisher = Substitute.For<IEmailPublisher>();
        _handler = new MessageAddedHandler(_companiesApiClient, _emailPublisher);
    }
    #endregion
}