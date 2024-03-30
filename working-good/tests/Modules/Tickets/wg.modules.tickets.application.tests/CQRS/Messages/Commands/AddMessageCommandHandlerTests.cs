using NSubstitute;
using Shouldly;
using wg.modules.tickets.application.Clients.Owner;
using wg.modules.tickets.application.Clients.Owner.DTO;
using wg.modules.tickets.application.CQRS.Messages.Commands.AddMessage;
using wg.modules.tickets.application.Events;
using wg.modules.tickets.application.Exceptions;
using wg.modules.tickets.domain.Exceptions;
using wg.modules.tickets.domain.Repositories;
using wg.modules.tickets.domain.Services;
using wg.modules.tickets.domain.ValueObjects.Ticket;
using wg.shared.abstractions.Messaging;
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
    public async Task AddMessage_GivenAddMessageCommandWithExistingTicketId_ShouldBeUpdatedByRepositoryAndSentByMessageBroker()
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
        var recipients = ticket.Messages.Select(x => x.Sender.Value).ToArray();
        await _messageBroker
            .Received(1)
            .PublishAsync(Arg.Is<MessageAdded>(arg
                => arg.TicketNumber == ticket.Number
                   && arg.Subject == ticket.Subject
                   && arg.Content == command.Content
                   && arg.Recipients[0] == recipients[0]
                   ));
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
    private readonly IClock _clock;
    private readonly INewMessageDomainService _newMessageDomainService;
    private readonly IMessageBroker _messageBroker;
    private readonly AddMessageCommandHandler _handler;

    public AddMessageCommandHandlerTests()
    {
        _ownerApiClient = Substitute.For<IOwnerApiClient>();
        _ticketRepository = Substitute.For<ITicketRepository>();
        _clock = TestsClock.Create();
        _newMessageDomainService = new NewMessageDomainService(_ticketRepository);
        _messageBroker = Substitute.For<IMessageBroker>();
        _handler = new AddMessageCommandHandler(_ownerApiClient, _clock, _newMessageDomainService,
            _messageBroker);
    }
    #endregion
}