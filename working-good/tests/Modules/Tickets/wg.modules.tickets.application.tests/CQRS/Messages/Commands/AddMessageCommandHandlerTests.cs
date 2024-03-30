using NSubstitute;
using Shouldly;
using wg.modules.tickets.application.Clients.Companies;
using wg.modules.tickets.application.Clients.Owner;
using wg.modules.tickets.application.Clients.Owner.DTO;
using wg.modules.tickets.application.CQRS.Messages.Commands.AddMessage;
using wg.modules.tickets.application.Exceptions;
using wg.modules.tickets.domain.Exceptions;
using wg.modules.tickets.domain.Repositories;
using wg.modules.tickets.domain.Services;
using wg.modules.tickets.domain.ValueObjects.Ticket;
using wg.shared.abstractions.Time;
using wg.tests.shared.Factories.DTOs.Tickets;
using wg.tests.shared.Factories.Tickets;
using wg.tests.shared.Mocks;
using Xunit;

namespace wg.modules.tickets.application.tests.CQRS.Messages.Commands;

public sealed class AddMessageCommandHandlerTests
{
    private Task Act(AddMessageCommand command) => _handler.HandleAsync(command, default);

    [Fact]
    public async Task AddMessage_GivenAddMessageCommandWithExistingTicketId_ShouldBeUpdatedByRepository()
    {
        //arrange
        var ticket = TicketsFactory.GetAll(State.New());
        var userDto = UserDtoFactory.Get().Single();
        var command = new AddMessageCommand(Guid.NewGuid(), userDto.Id, "My test content",
            ticket.Id);
        _ticketRepository
            .GetByIdAsync(ticket.Id)
            .Returns(ticket);
        _ownerApiClient
            .GetUserByIdAsync(new UserIdDto(command.UserId))
            .Returns(userDto);
        
        //act
        await Act(command);
        
        //assert
        await _ticketRepository
            .Received(1)
            .UpdateAsync(ticket);
        
        var message = ticket.Messages.FirstOrDefault(x => x.Id.Equals(command.Id));
        message.ShouldNotBeNull();
    }

    [Fact]
    public async Task AddMessage_GivenNotExistingTicketId_ShouldThrowTicketNotFoundException()
    {
        //arrange
        var userDto = UserDtoFactory.Get().Single();
        var command = new AddMessageCommand(Guid.NewGuid(), userDto.Id, "My test content",
            Guid.NewGuid());
        _ownerApiClient
            .GetUserByIdAsync(new UserIdDto(command.UserId))
            .Returns(userDto);
        
        //act
        var exception = await Record.ExceptionAsync(async () => await Act(command));
        
        //assert
        exception.ShouldBeOfType<TicketNotFoundException>();
    }

    [Fact]
    public async Task AddMessage_GivenNotExistingUserId_ShouldThrowUserNotFoundException()
    {
        //arrange
        var command = new AddMessageCommand(Guid.NewGuid(), Guid.NewGuid(), "My test content",
            Guid.NewGuid());
        
        //act
        var exception = await Record.ExceptionAsync(async () => await Act(command));
        
        //assert
        exception.ShouldBeOfType<UserNotFoundException>();
    }
    
    #region arrange
    private readonly IOwnerApiClient _ownerApiClient;
    private readonly ITicketRepository _ticketRepository;
    private readonly DateTime _now;
    private readonly IClock _clock;
    private readonly INewMessageDomainService _newMessageDomainService;
    private readonly AddMessageCommandHandler _handler;

    public AddMessageCommandHandlerTests()
    {
        _ownerApiClient = Substitute.For<IOwnerApiClient>();
        _ticketRepository = Substitute.For<ITicketRepository>();
        _now = DateTime.Now;
        _clock = TestsClock.Create(_now);
        _newMessageDomainService = new NewMessageDomainService(_ticketRepository);
        _handler = new AddMessageCommandHandler(_ownerApiClient, _clock, _newMessageDomainService);
    }
    #endregion
}