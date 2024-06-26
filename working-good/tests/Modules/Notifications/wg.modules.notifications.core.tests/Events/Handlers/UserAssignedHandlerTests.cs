using NSubstitute;
using wg.modules.notifications.core.Cache;
using wg.modules.notifications.core.Clients.Owner;
using wg.modules.notifications.core.Clients.Owner.DTO;
using wg.modules.notifications.core.Events.External;
using wg.modules.notifications.core.Events.External.Handlers;
using wg.modules.notifications.core.Models;
using wg.modules.notifications.core.Providers.Abstractions;
using wg.modules.notifications.core.Services.Abstractions;
using wg.shared.abstractions.Events;
using wg.tests.shared.Factories.DTOs.Notifications;
using Xunit;

namespace wg.modules.notifications.core.tests.Events.Handlers;

public sealed class UserAssignedHandlerTests
{
    private Task Act(UserAssigned @event) => _handler.HandleAsync(@event);

    [Fact]
    public async Task HandleAsync_GivenEvent_ShouldSendByMailPublisher()
    {
        //arrange
        var userDto = UserDtoFactory.Get();
        var @event = new UserAssigned(Guid.NewGuid(), 123, userDto.Id);

        _ownerApiClient
            .GetUserAsync(Arg.Is<UserIdDto>(arg => arg.Id == @event.UserId))
            .Returns(userDto);
        
        var notification = new EmailNotification()
        {
            Content = "Test",
            Subject = "Test",
            Recipient = [userDto.Email]
        };
        
        _emailNotificationProvider
            .GetForAssigning(userDto.Email, @event.TicketNumber)
            .Returns(notification);
        
        //act
        await Act(@event);
        
        //assert
        await _emailPublisher
            .Received(1)
            .PublishAsync(Arg.Is<EmailNotification>(arg => arg.Recipient[0] == userDto.Email),
                default);
    }
    
    #region arrange
    private readonly IOwnerApiClient _ownerApiClient;
    private readonly IEmailPublisher _emailPublisher;
    private readonly IEmailNotificationProvider _emailNotificationProvider;
    private readonly IEventHandler<UserAssigned> _handler;
    
    public UserAssignedHandlerTests()
    {
        _ownerApiClient = Substitute.For<IOwnerApiClient>();
        _emailPublisher = Substitute.For<IEmailPublisher>();
        _emailNotificationProvider = Substitute.For<IEmailNotificationProvider>();
        _handler = new UserAssignedHandler(_ownerApiClient, _emailNotificationProvider,
            _emailPublisher);
    }
    #endregion
}