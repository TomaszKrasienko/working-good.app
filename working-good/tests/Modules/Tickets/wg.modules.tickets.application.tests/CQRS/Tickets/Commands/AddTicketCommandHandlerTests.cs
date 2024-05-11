using NSubstitute;
using Shouldly;
using wg.modules.tickets.application.Clients.Companies;
using wg.modules.tickets.application.Clients.Companies.DTO;
using wg.modules.tickets.application.Clients.Owner;
using wg.modules.tickets.application.Clients.Owner.DTO;
using wg.modules.tickets.application.CQRS.Tickets.Commands.AddTicket;
using wg.modules.tickets.application.Events;
using wg.modules.tickets.application.Exceptions;
using wg.modules.tickets.domain.Entities;
using wg.modules.tickets.domain.Repositories;
using wg.modules.tickets.domain.ValueObjects.Ticket;
using wg.shared.abstractions.Events;
using wg.shared.abstractions.Messaging;
using wg.shared.abstractions.Time;
using wg.tests.shared.Factories.DTOs.Tickets.Company;
using wg.tests.shared.Factories.DTOs.Tickets.Owner;
using wg.tests.shared.Factories.Owners;
using wg.tests.shared.Factories.Tickets;
using wg.tests.shared.Mocks;
using Xunit;

namespace wg.modules.tickets.application.tests.CQRS.Tickets.Commands;

public sealed class AddTicketCommandHandlerTests
{
    private Task Act(AddTicketCommand command) => _handler.HandleAsync(command, default);

    [Fact]
    public async Task HandleAsync_GivenNotExistingTicketIdAndExistingCreatedByUser_ShouldAddNewTicketAndSendEvent()
    {
        //arrange
        var userDto = UserDtoFactory.Get();

        _ownerApiClient
            .GetActiveUserByIdAsync(Arg.Is<UserIdDto>(arg => arg.Id == userDto.Id))
            .Returns(userDto);

        var command = new AddTicketCommand(Guid.NewGuid(), "Test subject", "Test content", userDto.Id);

        await _ticketRepository
            .GetByIdAsync(command.Id);

        var maxNumber = new Random().Next(1000, 2000);
        _ticketRepository
            .GetMaxNumberAsync()
            .Returns(maxNumber);
        
        //act
        await Act(command);
        
        //assert
        await _ticketRepository
            .Received(1)
            .AddAsync(Arg.Is<Ticket>(arg
                => arg.Id.Value == command.Id
                   && arg.Subject.Value == command.Subject
                   && arg.Content.Value == command.Content
                   && arg.CreatedAt.Value == _now
                   && arg.CreatedBy.Value == userDto.Email
                   && arg.Number.Value == maxNumber + 1));
    }
    
    [Fact]
    public async Task HandleAsync_GivenExistingTicketId_ShouldThrowTicketAlreadyRegisteredException()
    {
        //arrange
        var command = new AddTicketCommand(Guid.NewGuid(), "Test subject", "Test content",
            Guid.NewGuid());

        _ticketRepository
            .GetByIdAsync(command.Id)
            .Returns(TicketsFactory.Get());
        
        //act
        var exception = await Record.ExceptionAsync(async () => await Act(command));
        
        //assert
        exception.ShouldBeOfType<TicketAlreadyRegisteredException>();
    }
    
    [Fact]
    public async Task HandleAsync_GivenNotExistingCreatedBy_ShouldThrowAuthorUserNotFoundException()
    {
        //arrange
        await _ownerApiClient
            .GetActiveUserByIdAsync(Arg.Any<UserIdDto>());

        var command = new AddTicketCommand(Guid.NewGuid(), "Test subject", "Test content", 
            Guid.NewGuid());

        await _ticketRepository
            .GetByIdAsync(command.Id);
        
        //act
        var exception = await Record.ExceptionAsync(() => Act(command));
        
        //assert
        exception.ShouldBeOfType<AuthorUserNotFoundException>();
    }
    
    
    #region arrange
    private readonly ITicketRepository _ticketRepository;
    private readonly IOwnerApiClient _ownerApiClient;
    private readonly DateTime _now;
    private readonly AddTicketCommandHandler _handler;

    public AddTicketCommandHandlerTests()
    {
        _ticketRepository = Substitute.For<ITicketRepository>();
        _ownerApiClient = Substitute.For<IOwnerApiClient>();
        _now = DateTime.Now;
        var clock = TestsClock.Create(_now);
        _handler = new AddTicketCommandHandler(_ticketRepository, _ownerApiClient,
          clock);
    }
    #endregion
                      
}