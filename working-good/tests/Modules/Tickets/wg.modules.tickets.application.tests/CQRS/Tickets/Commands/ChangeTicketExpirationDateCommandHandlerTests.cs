using NSubstitute;
using Shouldly;
using wg.modules.tickets.application.Clients.Companies;
using wg.modules.tickets.application.CQRS.Tickets.Commands.ChangeTicketExpirationDate;
using wg.modules.tickets.domain.Exceptions;
using wg.modules.tickets.domain.Repositories;
using wg.shared.abstractions.CQRS.Commands;
using Xunit;

namespace wg.modules.tickets.application.tests.CQRS.Tickets.Commands;

public sealed class ChangeTicketExpirationDateCommandHandlerTests
{
    private Task Act(ChangeTicketExpirationDateCommand command) => _handler.HandleAsync(command, default);

    [Fact]
    public async Task HandleAsync_GivenNotExistingTicket_ShouldThrowTicketNotFoundException()
    {
        //arrange
        var command = new ChangeTicketExpirationDateCommand(Guid.NewGuid(), DateTime.Now.AddHours(8));

        //act
        var exception = await Record.ExceptionAsync(async () => await Act(command));

        //assert
        exception.ShouldBeOfType<TicketNotFoundException>();
    }

    #region arrange
    private readonly ITicketRepository _ticketRepository;
    private readonly ICompaniesApiClient _companiesApiClient;
    private readonly ICommandHandler<ChangeTicketExpirationDateCommand> _handler;
    
    public ChangeTicketExpirationDateCommandHandlerTests()
    {
        _ticketRepository = Substitute.For<ITicketRepository>();
        _companiesApiClient = Substitute.For<ICompaniesApiClient>();
        _handler = new ChangeTicketExpirationDateCommandHandler(_ticketRepository, _companiesApiClient);
    }
    #endregion
}