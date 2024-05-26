using NSubstitute;
using wg.modules.companies.domain.Entities;
using wg.modules.notifications.core.Cache;
using wg.modules.notifications.core.Clients.Companies;
using wg.modules.notifications.core.Events.External;
using wg.modules.notifications.core.Events.External.Handlers;
using wg.modules.notifications.core.Providers.Abstractions;
using wg.shared.abstractions.Events;
using Xunit;

namespace wg.modules.notifications.core.tests.Events.Handlers;

public sealed class EmployeeAddedHandlerTests
{
    [Fact]
    public async Task HandleAsync_GivenEmployeeAddedEvent_ShouldSendByEmailPublisherAndAddToCache()
    {
        //arrange
        var @event = new EmployeeAdded(Guid.NewGuid(), "joe.doe@email.pl");
        
      
    }
    
    #region arrange
    private readonly ICacheService _cacheService;
    private readonly ICompaniesApiClient _companiesApiClient;
    private readonly IEmailNotificationProvider _emailNotificationProvider;
    private readonly IEventHandler<EmployeeAdded> _handler;

    public EmployeeAddedHandlerTests()
    {
        _cacheService = Substitute.For<ICacheService>();
        _companiesApiClient = Substitute.For<ICompaniesApiClient>();
        _emailNotificationProvider = Substitute.For<IEmailNotificationProvider>();
        _handler = new EmployeeAddedHandler();
    }
    #endregion
}