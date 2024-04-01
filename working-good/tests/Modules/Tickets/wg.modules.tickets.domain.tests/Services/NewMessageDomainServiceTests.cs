using NSubstitute;
using Shouldly;
using wg.modules.tickets.domain.Entities;
using wg.modules.tickets.domain.Exceptions;
using wg.modules.tickets.domain.Repositories;
using wg.modules.tickets.domain.Services;
using wg.modules.tickets.domain.ValueObjects.Ticket;
using wg.tests.shared.Factories.Tickets;
using Xunit;

namespace wg.modules.tickets.domain.tests.Services;

public sealed class NewMessageDomainServiceTests
{
    [Fact]
    public async Task AddNewMessage_GivenNullNumberAndId_ShouldAddNewTicketByRepositoryAndReturnTicket()
    {
        //arrange
        var id = Guid.NewGuid();
        var sender = "joe.doe@test.pl";
        var subject = "My Test Subject";
        var content = "My Test Content";
        var employeeId = Guid.NewGuid();
        var createdAt = DateTime.Now;
        var maxNumber = 1;
        _ticketRepository
            .GetMaxNumberAsync()
            .Returns(1);
        
        //act
        var result = await _service.AddNewMessage(id, sender, subject, content, createdAt, 
            null, null, employeeId);
        
        //assert
        await _ticketRepository
            .Received(1)
            .AddAsync(Arg.Is<Ticket>(x
                => x.Id.Equals(id)
                   && x.Subject.Value == subject
                   && x.Content.Value == content
                   && x.CreatedAt.Equals(createdAt)
                   && x.AssignedEmployee.Equals(employeeId)
                   && x.Number.Value == maxNumber + 1));
        result.ShouldNotBeNull();
    }

    [Fact]
    public async Task AddNewMessage_GivenTicketNumber_ShouldAddMessageToTicketAndUpdate()
    {
        //arrange
        var ticket = TicketsFactory.GetOnlyRequired(State.New());
        var id = Guid.NewGuid();
        var sender = "joe.doe@test.pl";
        var subject = "My Test Subject";
        var content = "My Test Content";
        var createdAt = DateTime.Now;
        _ticketRepository
            .GetByNumberAsync(ticket.Number)
            .Returns(ticket);
        
        //act
        var result = await _service.AddNewMessage(id, sender, subject, content, createdAt, ticket.Number.Value, null, null);
        
        //assert
        var updatedTicket = ticket.Messages.FirstOrDefault(x => x.Id.Equals(id));
        updatedTicket.ShouldNotBeNull();
        updatedTicket.Sender.Value.ShouldBe(sender);
        updatedTicket.Subject.Value.ShouldBe(subject);
        updatedTicket.Content.Value.ShouldBe(content);
        updatedTicket.CreatedAt.Value.ShouldBe(createdAt);
        await _ticketRepository
            .Received(1)
            .UpdateAsync(ticket);
        result.ShouldNotBeNull();
    }
    
    [Fact]
    public async Task AddNewMessage_GivenTicketId_ShouldAddMessageToTicketAndUpdate()
    {
        //arrange
        var ticket = TicketsFactory.GetOnlyRequired(State.New());
        var id = Guid.NewGuid();
        var sender = "joe.doe@test.pl";
        var subject = "My Test Subject";
        var content = "My Test Content";
        var createdAt = DateTime.Now;
        _ticketRepository
            .GetByIdAsync(ticket.Id)
            .Returns(ticket);
        
        //act
        var result = await _service.AddNewMessage(id, sender, subject, content, createdAt, null, ticket.Id, null);
        
        //assert
        var updatedTicket = ticket.Messages.FirstOrDefault(x => x.Id.Equals(id));
        updatedTicket.ShouldNotBeNull();
        updatedTicket.Sender.Value.ShouldBe(sender);
        updatedTicket.Subject.Value.ShouldBe(subject);
        updatedTicket.Content.Value.ShouldBe(content);
        updatedTicket.CreatedAt.Value.ShouldBe(createdAt);
        await _ticketRepository
            .Received(1)
            .UpdateAsync(ticket);
        result.ShouldNotBeNull();
    }
    
    [Fact]
    public async Task AddNewMessage_GivenTicketIdAndNullSubject_ShouldAddMessageToTicketAndUpdateWithSubjectFromTicket()
    {
        //arrange
        var ticket = TicketsFactory.GetOnlyRequired(State.New());
        var id = Guid.NewGuid();
        var sender = "joe.doe@test.pl";
        var content = "My Test Content";
        var createdAt = DateTime.Now;
        _ticketRepository
            .GetByIdAsync(ticket.Id)
            .Returns(ticket);
        
        //act
        var result = await _service.AddNewMessage(id, sender, null, content, createdAt, null, ticket.Id, null);
        
        //assert
        var updatedTicket = ticket.Messages.FirstOrDefault(x => x.Id.Equals(id));
        updatedTicket.ShouldNotBeNull();
        updatedTicket.Sender.Value.ShouldBe(sender);
        updatedTicket.Subject.Value.ShouldBe(ticket.Subject);
        updatedTicket.Content.Value.ShouldBe(content);
        updatedTicket.CreatedAt.Value.ShouldBe(createdAt);
        await _ticketRepository
            .Received(1)
            .UpdateAsync(ticket);
        result.ShouldNotBeNull();
    }
    
    [Fact]
    public async Task AddNewMessage_GivenNotExistingNumber_ShouldThrowTicketNotFound()
    {
        //act
        var exception = await Record.ExceptionAsync(async () => await _service.AddNewMessage(Guid.NewGuid(), "joe@doe.pl",
            "Test subject", "Test content", DateTime.Now, 1, null, null));
        
        //assert
        exception.ShouldBeOfType<TicketNotFoundException>();
    }
    
    [Fact]
    public async Task AddNewMessage_GivenNotExistingTicketId_ShouldThrowTicketNotFound()
    {
        //act
        var exception = await Record.ExceptionAsync(async () => await _service.AddNewMessage(Guid.NewGuid(), "joe@doe.pl",
            "Test subject", "Test content", DateTime.Now, null, Guid.NewGuid(), null));
        
        //assert
        exception.ShouldBeOfType<TicketNotFoundException>();
    }
    
    #region arrange
    private readonly ITicketRepository _ticketRepository;
    private readonly INewMessageDomainService _service;
    
    public NewMessageDomainServiceTests()
    {
        _ticketRepository = Substitute.For<ITicketRepository>();
        _service = new NewMessageDomainService(_ticketRepository);
    }
    #endregion
}