using NSubstitute;
using wg.modules.tickets.application.Clients.Owner;
using wg.modules.tickets.application.CQRS.Activities.Commands.AddActivity;
using wg.modules.tickets.domain.Repositories;

namespace wg.modules.tickets.application.tests.CQRS.Activities.Commands;

public sealed class AddActivityCommandHandlerTests
{
    #region arrange

    private readonly ITicketRepository _ticketRepository;
    private readonly IOwnerApiClient _ownerApiClient;
    private readonly AddActivityCommandHandler _handler;

    public AddActivityCommandHandlerTests()
    {
        _ticketRepository = Substitute.For<ITicketRepository>();
        _ownerApiClient = Substitute.For<IOwnerApiClient>();
        _handler = new AddActivityCommandHandler(_ticketRepository, _ownerApiClient);
    }
    #endregion
}