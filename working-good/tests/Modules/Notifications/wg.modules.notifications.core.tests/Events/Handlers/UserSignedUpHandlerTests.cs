using NSubstitute;
using wg.modules.notifications.core.Cache;
using wg.modules.notifications.core.Events.External;
using wg.modules.notifications.core.Events.External.Handlers;
using wg.modules.notifications.core.Models;
using wg.modules.notifications.core.Providers.Abstractions;
using wg.modules.notifications.core.Services.Abstractions;
using wg.shared.abstractions.Events;
using Xunit;

namespace wg.modules.notifications.core.tests.Events.Handlers;

public sealed class UserSignedUpHandlerTests
{
    private Task Act(UserSignedUp @event) => _handler.HandleAsync(@event);
    
    [Fact]
    public async Task HandleAsync_GivenEventWithAllFields_ShouldPublishEmailByEmailPublisher()
    {
        //arrange
        var @event = new UserSignedUp( Guid.NewGuid(),"joe.doe@test.pl", "Joe", "Doe", Guid.NewGuid().ToString());
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

        await _cacheService
            .Received(1)
            .Add(@event.Id.ToString(), @event.Email);
    }

    [Fact]
    public async Task HandleAsync_ForNullEmailNotification_ShouldNotPublishByEmailPublisher()
    {
        //arrange
        var @event = new UserSignedUp(Guid.NewGuid(), "joe.doe@test.pl", "Joe", "Doe", Guid.NewGuid().ToString());
        _emailNotificationProvider
            .GetForNewUser(@event.Email, @event.FirstName, @event.LastName, @event.VerificationToken);
        
        //act
        await Act(@event);
        
        //assert
        await _emailPublisher
            .Received(0)
            .PublishAsync(Arg.Any<EmailNotification>(), default);
        
        await _cacheService
            .Received(1)
            .Add(@event.Id.ToString(), @event.Email);
    }
    
    #region arrange
    private readonly IEmailNotificationProvider _emailNotificationProvider;
    private readonly IEmailPublisher _emailPublisher;
    private readonly ICacheService _cacheService;
    private readonly IEventHandler<UserSignedUp> _handler;
    
    public UserSignedUpHandlerTests()
    {
        _emailPublisher = Substitute.For<IEmailPublisher>();
        _emailNotificationProvider = Substitute.For<IEmailNotificationProvider>();
        _cacheService = Substitute.For<ICacheService>();
        _handler = new UserSignedUpHandler(_emailNotificationProvider, _emailPublisher,
            _cacheService);
    }
    #endregion
}