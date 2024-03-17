using Shouldly;
using wg.modules.tickets.domain.Entities;
using wg.modules.tickets.domain.Exceptions;
using wg.modules.tickets.domain.ValueObjects.Ticket;
using Xunit;

namespace wg.modules.tickets.domain.tests;

public sealed class TicketCreateTests
{
    [Fact]
    public void Create_ForAllArgumentsAndPriorityTrue_ShouldReturnTicketWithFilledFields()
    {
        //arrange
        var id = Guid.NewGuid();
        var number = 1;
        var subject = "Test ticket";
        var content = "Test content";
        var createdAt = DateTime.Now;
        var createdBy = Guid.NewGuid();
        var state = State.New(DateTime.Now);
        var isPriority = true;
        var expirationDate = DateTime.Now.AddDays(1);
        var assignedEmployee = Guid.NewGuid();
        var assignedUser = Guid.NewGuid();
        
        //act
        var result = Ticket.Create(id, number, subject, content, createdAt, createdBy, state.Value,
            state.ChangeDate, isPriority, expirationDate, assignedEmployee, assignedUser);
        
        //assert
        result.ShouldNotBeNull();
        result.Id.Value.ShouldBe(id);
        result.Number.Value.ShouldBe(number);
        result.Subject.Value.ShouldBe(subject);
        result.Content.Value.ShouldBe(content);
        result.CreatedAt.Value.ShouldBe(createdAt);
        result.CreatedBy.Value.ShouldBe(createdBy);
        result.State.ShouldBe(state);
        result.IsPriority.Value.ShouldBe(isPriority);
        result.ExpirationDate.Value.ShouldBe(expirationDate);
        result.AssignedEmployee.Value.ShouldBe(assignedEmployee);
        result.AssignedUser.Value.ShouldBe(assignedUser);
    }
    
    [Fact]
    public void Create_ForRequireArgumentsAndPriorityFalse_ShouldReturnTicketWithFilledFields()
    {
        //arrange
        var id = Guid.NewGuid();
        var number = 1;
        var subject = "Test ticket";
        var content = "Test content";
        var createdAt = DateTime.Now;
        var createdBy = Guid.NewGuid();
        var state = State.New(DateTime.Now);
        var isPriority = false;
        
        //act
        var result = Ticket.Create(id, number, subject, content, createdAt, createdBy, state.Value,
            state.ChangeDate, isPriority);
        
        //assert
        result.ShouldNotBeNull();
        result.Id.Value.ShouldBe(id);
        result.Number.Value.ShouldBe(number);
        result.Subject.Value.ShouldBe(subject);
        result.Content.Value.ShouldBe(content);
        result.CreatedAt.Value.ShouldBe(createdAt);
        result.CreatedBy.Value.ShouldBe(createdBy);
        result.State.ShouldBe(state);
        result.IsPriority.Value.ShouldBe(isPriority);
        result.ExpirationDate.ShouldBeNull();
        result.AssignedEmployee.ShouldBeNull();
        result.AssignedUser.ShouldBeNull();
    }
    
    [Fact]
    public void Create_GivenPriorityTrueAndExpirationDateNull_ShouldThrowMissingExpirationDateException()
    {
        //arrange
        var id = Guid.NewGuid();
        var number = 1;
        var subject = "Test ticket";
        var content = "Test content";
        var createdAt = DateTime.Now;
        var createdBy = Guid.NewGuid();
        var state = State.New(DateTime.Now);
        var isPriority = true;
        
        //act
        var exception = Record.Exception(() => Ticket.Create(id, number, subject, content, createdAt, createdBy, state.Value,
            state.ChangeDate, isPriority));
        
        //assert
        exception.ShouldBeOfType<MissingExpirationDateException>();
    }
}