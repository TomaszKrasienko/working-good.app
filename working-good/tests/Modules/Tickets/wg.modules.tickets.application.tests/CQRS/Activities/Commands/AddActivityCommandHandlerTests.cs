using NSubstitute;
using Shouldly;
using wg.modules.tickets.application.Clients.Owner;
using wg.modules.tickets.application.Clients.Owner.DTO;
using wg.modules.tickets.application.CQRS.Activities.Commands.AddActivity;
using wg.modules.tickets.application.Exceptions;
using wg.modules.tickets.domain.Exceptions;
using wg.modules.tickets.domain.Repositories;
using wg.modules.tickets.domain.ValueObjects.Ticket;
using wg.tests.shared.Factories.DTOs.Tickets;
using wg.tests.shared.Factories.DTOs.Tickets.Owner;
using wg.tests.shared.Factories.Tickets;
using Xunit;
using UserState = wg.modules.owner.domain.ValueObjects.User.State;

namespace wg.modules.tickets.application.tests.CQRS.Activities.Commands;

public sealed class AddActivityCommandHandlerTests
{
    private Task Act(AddActivityCommand command) => _handler.HandleAsync(command, default);

    [Fact]
    public async Task HandleAsync_GivenExistingTicketAndActiveUser_ShouldUpdateByRepository()
    {
        //arrange
        var ticket = TicketsFactory.GetAll(State.Open());
        var userDto = UserDtoFactory.Get(1).Single();
        
        var command = new AddActivityCommand(Guid.NewGuid(), ticket.Id,
            DateTime.Now.AddHours(-1), DateTime.Now.AddHours(1),
            "My activity", true, userDto.Id);

        _ticketRepository
            .GetByIdAsync(command.TicketId)
            .Returns(ticket);
        
        _ownerApiClient
            .GetActiveUserByIdAsync(Arg.Is<UserIdDto>(arg
                => arg.Id.Equals(command.UserId)))
            .Returns(userDto);
            
        //act
        await Act(command);
        
        //assert
        var result = ticket.Activities.FirstOrDefault(x => x.Id.Equals(command.Id));
        result.ShouldNotBeNull();
        
        await _ticketRepository
            .Received(1)
            .UpdateAsync(ticket);
    }

    [Fact]
    public async Task HandleAsync_GivenNotExistingTicketId_ShouldThrowTicketNotFoundException()
    {
        //arrange
        var command = new AddActivityCommand(Guid.NewGuid(), Guid.NewGuid(),
            DateTime.Now.AddHours(-1), DateTime.Now.AddHours(1),
            "My activity", true, Guid.NewGuid());
        
        //act
        var exception = await Record.ExceptionAsync(async () => await Act(command));
        
        //assert
        exception.ShouldBeOfType<TicketNotFoundException>();
    }
    
    [Fact]
    public async Task HandleAsync_GivenNotExistingActiveUserId_ShouldThrowActiveUserNotFoundException()
    {
        //arrange
        var ticket = TicketsFactory.GetAll(State.Open());
        var command = new AddActivityCommand(Guid.NewGuid(), ticket.Id,
            DateTime.Now.AddHours(-1), DateTime.Now.AddHours(1),
            "My activity", true, Guid.NewGuid());
        
        _ticketRepository
            .GetByIdAsync(command.TicketId)
            .Returns(ticket);
        
        //act
        var exception = await Record.ExceptionAsync(async () => await Act(command));
        
        //assert
        exception.ShouldBeOfType<ActiveUserNotFoundException>();
    } 
    
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