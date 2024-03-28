using NSubstitute;
using wg.modules.notifications.core.Clients.Companies;
using wg.modules.notifications.core.Clients.Companies.DTO;
using wg.modules.notifications.core.Events.External;
using wg.modules.notifications.core.Events.External.Handlers;
using wg.modules.notifications.core.Models;
using wg.modules.notifications.core.Providers.Abstractions;
using wg.modules.notifications.core.Services.Abstractions;
using wg.tests.shared.Factories.Events;
using Xunit;

namespace wg.modules.notifications.core.tests.Events.Handlers;

public sealed class TicketCreatedHandlerTests
{
    private Task Act(TicketCreated @event) => _handler.HandleAsync(@event);
    
    [Fact]
    public async Task HandleAsync_GivenFilledEmployeeId_ShouldPublishByEmailPublisher()
    {
        //arrange
        var @event = TicketCreatedFactory.Get(false, true);
        var employeeEmail = "test@test.pl";
        _companiesApiClient
            .GetEmployeeEmail(Arg.Is<EmployeeIdDto>(arg
                => arg.Value == @event.EmployeeId))
            .Returns(new EmployeeEmailDto()
            {
                Value = employeeEmail
            });

        var notification = new EmailNotification()
        {
            Subject = $"#{@event.TicketNumber} - {@event.Subject}",
            Content = $"Created ticket with content: \n{@event.Content}",
            Recipient = employeeEmail
        };
        
        _emailNotificationProvider
            .GetForNewTicket(employeeEmail, @event.TicketNumber, @event.Content, @event.Subject)
            .Returns(notification);

        //act
        await Act(@event);
        
        //assert
        await _emailPublisher
            .Received(1)
            .PublishAsync(notification, default);
    }
    
    [Fact]
    public async Task HandleAsync_GivenNullEmployeeId_ShouldNotSendByClientAndPublishEmail()
    {
        //arrange
        var @event = TicketCreatedFactory.Get();
        
        //act
        await Act(@event);
        
        //assert
        await _companiesApiClient
            .Received(0)
            .GetEmployeeEmail(Arg.Any<EmployeeIdDto>());

        _emailNotificationProvider
            .Received(0)
            .GetForNewTicket(Arg.Any<string>(), Arg.Any<int>(), Arg.Any<string>(), Arg.Any<string>());

        await _emailPublisher
            .Received(0)
            .PublishAsync(Arg.Any<EmailNotification>(), default);
    }

    [Fact]
    public async Task HandleAsync_ForNullNotification_ShouldNotSendByPublisher()
    {
        //arrange
        var @event = TicketCreatedFactory.Get();
        
        var employeeEmail = "test@test.pl";
        _companiesApiClient
            .GetEmployeeEmail(Arg.Is<EmployeeIdDto>(arg
                => arg.Value == @event.EmployeeId))
            .Returns(new EmployeeEmailDto()
            {
                Value = employeeEmail
            });

        _emailNotificationProvider
            .GetForNewTicket(employeeEmail, @event.TicketNumber, @event.Content, @event.Subject);

        //act
        await Act(@event);
        
        //assert
        await _emailPublisher
            .Received(0)
            .PublishAsync(Arg.Any<EmailNotification>(), default);
    }


    
    #region arrange
    private readonly ICompaniesApiClient _companiesApiClient;
    private readonly IEmailNotificationProvider _emailNotificationProvider;
    private readonly IEmailPublisher _emailPublisher;
    private readonly TicketCreatedHandler _handler;

    public TicketCreatedHandlerTests()
    {
        _emailPublisher = Substitute.For<IEmailPublisher>();
        _companiesApiClient = Substitute.For<ICompaniesApiClient>();
        _emailNotificationProvider = Substitute.For<IEmailNotificationProvider>();
        _handler = new TicketCreatedHandler(_companiesApiClient, _emailNotificationProvider, _emailPublisher);
    }

    #endregion
}