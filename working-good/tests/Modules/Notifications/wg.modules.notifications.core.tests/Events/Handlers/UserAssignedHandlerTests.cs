using Microsoft.Extensions.Caching.Distributed;
using wg.modules.notifications.core.Cache;
using wg.modules.notifications.core.Services.Abstractions;

namespace wg.modules.notifications.core.tests.Events.Handlers;

public sealed class UserAssignedHandlerTests
{
    #region arrange

    private readonly ICacheService _distributedCache;
    private readonly IEmailPublisher _emailPublisher;
    #endregion
}