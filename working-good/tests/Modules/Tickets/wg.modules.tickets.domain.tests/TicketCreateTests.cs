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
        var state = State.New();
        var stateDate = DateTime.Now;
        var isPriority = true;
        var expirationDate = DateTime.Now.AddDays(1);
        var assignedEmployee = Guid.NewGuid();
        var assignedUser = Guid.NewGuid();
        
        //act
        var result = Ticket.Create(id, number, subject, content, createdAt, createdBy, state,
            stateDate, isPriority, expirationDate, assignedEmployee, assignedUser);
        
        //assert
        result.ShouldNotBeNull();
        result.Id.Value.ShouldBe(id);
        result.Number.Value.ShouldBe(number);
        result.Subject.Value.ShouldBe(subject);
        result.Content.Value.ShouldBe(content);
        result.CreatedAt.Value.ShouldBe(createdAt);
        result.CreatedBy.Value.ShouldBe(createdBy);
        result.State.Value.ShouldBe(state);
        result.State.ChangeDate.ShouldBe(stateDate);
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
        var state = State.New();
        var stateDate = DateTime.Now;
        var isPriority = false;
        
        //act
        var result = Ticket.Create(id, number, subject, content, createdAt, createdBy, state,
            stateDate, isPriority);
        
        //assert
        result.ShouldNotBeNull();
        result.Id.Value.ShouldBe(id);
        result.Number.Value.ShouldBe(number);
        result.Subject.Value.ShouldBe(subject);
        result.Content.Value.ShouldBe(content);
        result.CreatedAt.Value.ShouldBe(createdAt);
        result.CreatedBy.Value.ShouldBe(createdBy);
        result.State.Value.ShouldBe(state);
        result.State.ChangeDate.ShouldBe(stateDate);
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
        var state = State.New();
        var isPriority = true;
        
        //act
        var exception = Record.Exception(() => Ticket.Create(id, number, subject, content, createdAt, createdBy, state,
            DateTime.Now, isPriority));
        
        //assert
        exception.ShouldBeOfType<MissingExpirationDateException>();
    }

    [Fact]
    public void Create_GivenZeroNumber_ShouldThrowInvalidNumberException()
    {
        //act
        var exception = Record.Exception(() => Ticket.Create(Guid.NewGuid(), 0, "My subject", 
            "Test content", DateTime.Now, Guid.NewGuid(), State.New(),
            DateTime.Now, false));
        
        //assert
        exception.ShouldBeOfType<InvalidNumberException>();
    }

    [Fact]
    public void Create_GivenEmptySubject_ShouldThrowEmptySubjectException()
    {        
        //act
        var exception = Record.Exception(() => Ticket.Create(Guid.NewGuid(), 1, string.Empty, 
            "Test content", DateTime.Now, Guid.NewGuid(), State.New(),
            DateTime.Now, false));
        
        //assert
        exception.ShouldBeOfType<EmptySubjectException>();
    }
    
    [Fact]
    public void Create_GivenEmptyContent_ShouldThrowEmptyContentException()
    {        
        //act
        var exception = Record.Exception(() => Ticket.Create(Guid.NewGuid(), 1, "Test subject", 
            string.Empty, DateTime.Now, Guid.NewGuid(), State.New(),
            DateTime.Now, false));
        
        //assert
        exception.ShouldBeOfType<EmptyContentException>();
    }
    
    [Fact]
    public void Create_GivenEmptyState_ShouldThrowEmptyStateException()
    {        
        //act
        var exception = Record.Exception(() => Ticket.Create(Guid.NewGuid(), 1, "Test subject", 
            "Test content", DateTime.Now, Guid.NewGuid(), string.Empty,
            DateTime.Now, false));
        
        //assert
        exception.ShouldBeOfType<EmptyStateException>();
    }
    
    [Fact]
    public void Create_GivenInvalidState_ShouldThrowUnavailableStateException()
    {        
        //act
        var exception = Record.Exception(() => Ticket.Create(Guid.NewGuid(), 1, "Test subject", 
            "Test content", DateTime.Now, Guid.NewGuid(), "Invalid state",
            DateTime.Now, false));
        
        //assert
        exception.ShouldBeOfType<UnavailableStateException>();
    }
}