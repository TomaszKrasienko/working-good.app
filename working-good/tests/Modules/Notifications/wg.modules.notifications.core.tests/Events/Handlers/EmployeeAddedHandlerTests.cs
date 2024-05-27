using NSubstitute;
using wg.modules.companies.domain.Entities;
using wg.modules.notifications.core.Cache;
using wg.modules.notifications.core.Clients.Companies;
using wg.modules.notifications.core.Clients.Companies.DTO;
using wg.modules.notifications.core.Events.External;
using wg.modules.notifications.core.Events.External.Handlers;
using wg.modules.notifications.core.Models;
using wg.modules.notifications.core.Providers.Abstractions;
using wg.modules.notifications.core.Services.Abstractions;
using wg.shared.abstractions.Events;
using wg.tests.shared.Factories.DTOs.Notifications;
using Xunit;

namespace wg.modules.notifications.core.tests.Events.Handlers;

public sealed class EmployeeAddedHandlerTests
{
    private Task Act(EmployeeAdded employeeAdded) => _handler.HandleAsync(employeeAdded);
    
    [Fact]
    public async Task HandleAsync_GivenEmployeeAddedEvent_ShouldSendByEmailPublisherAndAddToCache()
    {
        //arrange
        var employeeDto = EmployeeDtoFactory.Get();
        var @event = new EmployeeAdded(employeeDto.Id, employeeDto.Email);

        _companiesApiClient
            .GetEmployeeByIdAsync(new EmployeeIdDto() { Id = @event.Id })
            .Returns(employeeDto);

        var emailNotification = new EmailNotification()
        {
            Recipient = [@event.Email],
            Content = "test content",
            Subject = "test subject"
        };
        
        _emailNotificationProvider
            .GetForNewEmployee(employeeDto.Email)
            .Returns(emailNotification);
        
        //act
        await Act(@event);
        
        //assert
        await _emailPublisher
            .Received(1)
            .PublishAsync(emailNotification, default);
    }
    
    #region arrange
    private readonly ICompaniesApiClient _companiesApiClient;
    private readonly IEmailNotificationProvider _emailNotificationProvider;
    private readonly IEmailPublisher _emailPublisher;
    private readonly IEventHandler<EmployeeAdded> _handler;

    public EmployeeAddedHandlerTests()
    {
        _companiesApiClient = Substitute.For<ICompaniesApiClient>();
        _emailNotificationProvider = Substitute.For<IEmailNotificationProvider>();
        _emailPublisher = Substitute.For<IEmailPublisher>();
        _handler = new EmployeeAddedHandler(_companiesApiClient, _emailNotificationProvider, 
            _emailPublisher);
    }
    #endregion
}