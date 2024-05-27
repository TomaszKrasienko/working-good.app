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

public sealed class UserSignedUpHandlerTests
{
    private Task Act(UserSignedUp @event) => _handler.HandleAsync(@event);
    
    [Fact]
    public async Task HandleAsync_GivenEventWithAllFields_ShouldPublishEmailByEmailPublisher()
    {
        //arrange\
        var userDto = UserDtoFactory.Get();

        _ownerApiClient
            .GetUserAsync(Arg.Is<UserIdDto>(arg => arg.Id == userDto.Id))
            .Returns(userDto);
        
        var @event = new UserSignedUp( userDto.Id, userDto.Email, userDto.FirstName, userDto.LastName, Guid.NewGuid().ToString());
        var emailNotification = new EmailNotification()
        {
            Recipient = [@event.Email],
            Subject = "Test first user subject",
            Content = $"New user {@event.FirstName} {@event.LastName}, verificationToken: {@event.VerificationToken}"
        };

        _emailNotificationProvider
            .GetForNewUser(@event.Email, @event.FirstName, @event.LastName, @event.VerificationToken)
            .Returns(emailNotification);

        //act
        await Act(@event);

        //assert
        await _emailPublisher
            .Received(1)
            .PublishAsync(emailNotification, default);
    }
    
    #region arrange
    private readonly IEmailNotificationProvider _emailNotificationProvider;
    private readonly IEmailPublisher _emailPublisher;
    private readonly IOwnerApiClient _ownerApiClient;
    private readonly IEventHandler<UserSignedUp> _handler;
    
    public UserSignedUpHandlerTests()
    {
        _emailPublisher = Substitute.For<IEmailPublisher>();
        _emailNotificationProvider = Substitute.For<IEmailNotificationProvider>();
        _ownerApiClient = Substitute.For<IOwnerApiClient>();
        _handler = new UserSignedUpHandler(_emailNotificationProvider, _emailPublisher,
            _ownerApiClient);
    }
    #endregion
}