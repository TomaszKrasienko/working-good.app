using System.ComponentModel.DataAnnotations.Schema;
using NSubstitute;
using Shouldly;
using wg.modules.tickets.application.Clients.Companies;
using wg.modules.tickets.application.Clients.Companies.DTO;
using wg.modules.tickets.application.CQRS.Tickets.Commands.ChangeProject;
using wg.modules.tickets.domain.Repositories;
using wg.shared.abstractions.CQRS.Commands;
using wg.tests.shared.Factories.Tickets;
using Xunit;

namespace wg.modules.tickets.application.tests.CQRS.Tickets.Commands;

public sealed class ChangeProjectCommandHandlerTests
{
    private Task Act(ChangeProjectCommand command) => _handler.HandleAsync(command, default);

    [Fact]
    public async Task HandleAsync_GivenExistingTicketAndNotAssignedEmployeeAndUser_ShouldChangeProject()
    {
        //arrange
        var ticket = TicketsFactory.Get();
        var projectId = Guid.NewGuid();
        var command = new ChangeProjectCommand(ticket.Id, projectId);
        _ticketRepository
            .GetByIdAsync(command.TicketId)
            .Returns(ticket);

        _companiesApiClient
            .IsProjectActiveAsync(Arg.Is<ProjectIdDto>(arg => arg.Id.Equals(command.ProjectId)))
            .Returns(new IsProjectActiveDto()
            {
                Value = true
            });
        
        //act
        await Act(command);
        
        //assert 
        await _ticketRepository
            .Received(1)
            .UpdateAsync(ticket);
        
        ticket.ProjectId.Value.ShouldBe(command.ProjectId);
    }
    
    #region arrage
    private readonly ITicketRepository _ticketRepository;
    private readonly ICompaniesApiClient _companiesApiClient;
    private readonly ICommandHandler<ChangeProjectCommand> _handler;

    public ChangeProjectCommandHandlerTests()
    {
        _ticketRepository = Substitute.For<ITicketRepository>();
        _companiesApiClient = Substitute.For<ICompaniesApiClient>();
        _handler = new ChangeProjectCommandHandler(_ticketRepository, _companiesApiClient);
    }
    #endregion
}