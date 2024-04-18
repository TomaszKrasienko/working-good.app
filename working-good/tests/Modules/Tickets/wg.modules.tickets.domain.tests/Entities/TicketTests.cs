using Shouldly;
using wg.modules.tickets.domain.Exceptions;
using wg.modules.tickets.domain.ValueObjects.Ticket;
using wg.tests.shared.Factories.Tickets;
using Xunit;

namespace wg.modules.tickets.domain.tests.Entities;

public sealed class TicketTests
{
    [Fact]
    public void ChangeAssignedEmployee_ForTicketWithStatusToAssigning_ShouldChangeAssignedEmployee()
    {
        //arrange
        var ticket = TicketsFactory.GetAll(state: State.Open());
        var substituteEmployeeId = Guid.NewGuid();
        
        //act
        ticket.ChangeAssignedEmployee(substituteEmployeeId);
        
        //assert
        ticket.AssignedEmployee.Value.ShouldBe(substituteEmployeeId);
    }
    
    [Fact]
    public void ChangeAssignedEmployee_ForTicketWithStatusToNotAssigning_ShouldNotChangeAssignedEmployee()
    {
        //arrange
        var ticket = TicketsFactory.GetAll(state: State.Done());
        var oldAssignedEmployee = ticket.AssignedEmployee;
        
        //act
        ticket.ChangeAssignedEmployee(Guid.NewGuid());
        
        //assert
        ticket.AssignedEmployee.ShouldBe(oldAssignedEmployee);
    }

    [Fact]
    public void ChangeAssignedUser_GivenUserIdAndDateForStateNew_ShouldChangeAssignedUserAndStateToOpenAndStateDate()
    {
        //arrange
        var ticket = TicketsFactory.GetOnlyRequired(state: State.New()).Single();
        var userId = Guid.NewGuid();
        var date = DateTime.Now;
        
        //act
        ticket.ChangeAssignedUser(userId, date);
        
        //assert
        ticket.AssignedUser.Value.ShouldBe(userId);
        ticket.State.Value.ShouldBe(State.Open());
        ticket.State.ChangeDate.ShouldBe(date);
    }
    
    [Fact]
    public void ChangeAssignedUser_GivenUserIdAndDateForStateInProgress_ShouldChangeAssignedUserAndNotChangeState()
    {
        //arrange
        var ticket = TicketsFactory.GetOnlyRequired(state: State.InProgress()).Single();
        var currentStateDate = ticket.State.ChangeDate;
        var userId = Guid.NewGuid();
        var date = DateTime.Now;
        
        //act
        ticket.ChangeAssignedUser(userId, date);
        
        //assert
        ticket.AssignedUser.Value.ShouldBe(userId);
        ticket.State.Value.ShouldBe(State.InProgress());
        ticket.State.ChangeDate.ShouldBe(currentStateDate);
    }

    [Fact]
    public void ChangeAssignedUser_ForCancelledState_ShouldNotChangeState()
    {        
        //arrange
        var ticket = TicketsFactory.GetOnlyRequired(state: State.Cancelled()).Single();
        var originalState = ticket.State;
        var userId = Guid.NewGuid();
        var date = DateTime.Now;
        
        //act
        ticket.ChangeAssignedUser(userId, date);
        
        //assert
        ticket.State.ShouldBe(originalState);
    }
    
    [Fact]
    public void ChangeAssignedEmployee_GivenEmployeeIddForStateInProgress_ShouldChangeAssignedEmployee()
    {
        //arrange
        var ticket = TicketsFactory.GetOnlyRequired(state: State.InProgress()).Single();
        var employeeId = Guid.NewGuid();
        
        //act
        ticket.ChangeAssignedEmployee(employeeId);
        
        //assert
        ticket.AssignedEmployee.Value.ShouldBe(employeeId);
    }

    [Fact]
    public void ChangeAssignedUser_ForCancelledState_ShouldNotChangeAssigning()
    {        
        //arrange
        var ticket = TicketsFactory.GetOnlyRequired(state: State.Cancelled()).Single();
        var originalEmployee = ticket.AssignedEmployee;
        var employeeId = Guid.NewGuid();
        
        //act
        ticket.ChangeAssignedEmployee(employeeId);
        
        //assert
        ticket.AssignedEmployee.ShouldBe(originalEmployee);
    }
    
    [Fact]
    public void ChangeProject_GivenProjectId_ChangeProjectId()
    {        
        //arrange
        var ticket = TicketsFactory.GetOnlyRequired(state: State.Cancelled()).Single();
        var projectId = Guid.NewGuid();
        
        //act
        ticket.ChangeProject(projectId);
        
        //assert
        ticket.ProjectId.Value.ShouldBe(projectId);
    }
    
    [Fact]
    public void AddMessage_GivenMessageAnd_ShouldAddToMessages()
    {
        //arrange
        var ticket = TicketsFactory.GetOnlyRequired(state: State.New()).Single();
        var id = Guid.NewGuid();
        var sender = "joe@doe.pl";
        var subject = "Test subject";
        var content = "Test content";
        var createdAt = DateTime.Now;
        
        //act
        ticket.AddMessage(id, sender, subject, content, createdAt);
        
        //assert
        var message = ticket.Messages.FirstOrDefault(x => x.Id.Equals(id));
        ticket.State.Value.ShouldBe(State.Open());
        message.ShouldNotBeNull();
        message.Id.Value.ShouldBe(id);
        message.Sender.Value.ShouldBe(sender);
        message.Subject.Value.ShouldBe(subject);
        message.Content.Value.ShouldBe(content);
        message.CreatedAt.Value.ShouldBe(createdAt);
    }

    [Fact]
    public void AddActivity_GivenStatusForChanges_ShouldAddActivityToTicket()
    {
        //arrange
        var ticket = TicketsFactory.GetAll(state: State.Open());
        var activityId = Guid.NewGuid();
        
        //act
        ticket.AddActivity(activityId, DateTime.Now, DateTime.Now.AddMinutes(30),
            "Test note",true, Guid.NewGuid());
        
        //assert
        ticket.Activities.Any(x => x.Id.Equals(activityId)).ShouldBeTrue();
    }

    [Fact]
    public void AddActivity_GivenActivityWithCollisionTimeFromAndNullTimeToAndTheSameUser_ShouldThrowActivityHasCollisionDateTimeException()
    {
        //arrange
        var ticket = TicketsFactory.GetAll(state: State.Open());
        var existingActivity = ActivityFactory.GetInTicket(ticket, 1).Single();
        
        //act
        var exception = Record.Exception(() => ticket.AddActivity(Guid.NewGuid(), existingActivity.ActivityTime.TimeFrom.AddMinutes(1), null,
            "Test note",true, existingActivity.UserId));
        
        //assert
        exception.ShouldBeOfType<ActivityHasCollisionDateTimeException>();
    }
    
    [Fact]
    public void AddActivity_GivenActivityWithCollisionTimeToAndTheSameUser_ShouldThrowActivityHasCollisionDateTimeException()
    {
        //arrange
        var ticket = TicketsFactory.GetAll(state: State.Open());
        var existingActivity = ActivityFactory.GetInTicket(ticket, 1).Single();
        
        //act
        var exception = Record.Exception(() => ticket.AddActivity(Guid.NewGuid(), existingActivity.ActivityTime.TimeFrom.AddMinutes(-10),
            existingActivity.ActivityTime.TimeFrom.AddMinutes(10), "Test note",true, existingActivity.UserId));
        
        //assert
        exception.ShouldBeOfType<ActivityHasCollisionDateTimeException>();
    }
    
    [Fact]
    public void AddActivity_GivenActivityWithNullTimeToWithExistingActivityWithNullTimeTo_ShouldThrowActivityHasCollisionDateTimeException()
    {
        //arrange
        var ticket = TicketsFactory.GetAll(state: State.Open());
        var now = DateTime.Now;
        var userId = Guid.NewGuid();
        ticket.AddActivity(Guid.NewGuid(), now.AddMinutes(10), null, 
            "Note", true, userId);
        
        //act
        var exception = Record.Exception(() => ticket.AddActivity(Guid.NewGuid(), now.AddMinutes(20),
            null, "Test note",true, userId));
        
        //assert
        exception.ShouldBeOfType<ActivityHasCollisionDateTimeException>();
    }
    
    [Fact]
    public void AddActivity_GivenActivityWithExistingActivityWithNullTimeTo_ShouldThrowActivityHasCollisionDateTimeException()
    {
        //arrange
        var ticket = TicketsFactory.GetAll(state: State.Open());
        var now = DateTime.Now;
        var userId = Guid.NewGuid();
        ticket.AddActivity(Guid.NewGuid(), now.AddMinutes(10), null, 
            "Note", true, userId);
        
        //act
        var exception = Record.Exception(() => ticket.AddActivity(Guid.NewGuid(), now,
            now.AddMinutes(20), "Test note",true, userId));
        
        //assert
        exception.ShouldBeOfType<ActivityHasCollisionDateTimeException>();
    }
    
    [Fact]
    public void AddActivity_GivenStatusForChanges_ShouldThrowTicketHasNoStatusToAddActivityException()
    {
        //arrange
        var ticket = TicketsFactory.GetAll(state: State.Cancelled());
        
        //act
        var exception = Record.Exception(() => ticket.AddActivity(Guid.NewGuid(), 
            DateTime.Now, DateTime.Now.AddMinutes(30),
            "Test note",true, Guid.NewGuid()));
        
        //assert
        exception.ShouldBeOfType<TicketHasNoStatusToAddActivityException>();
    }

    [Fact]
    public void ChangeActivityType_GivenIsPaidActivity_ShouldMarkActivityIsPaidAsFalse()
    {
        //arrange
        var ticket = TicketsFactory.GetAll(state: State.Open());
        var activity = ActivityFactory.GetInTicket(ticket, isPaid: true).Single();

        //act
        ticket.ChangeActivityType(activity.Id);
        
        //assert
        activity.IsPaid.Value.ShouldBeFalse();
    }

    [Fact]
    public void ChangeActivityType_GivenNoExistingActivity_ShouldThrowActivityNotFoundException()
    {
        //arrange
        var ticket = TicketsFactory.GetAll(state: State.Open());

        //act
        var exception = Record.Exception(() => ticket.ChangeActivityType(Guid.NewGuid()));
        
        //assert
        exception.ShouldBeOfType<ActivityNotFoundException>();
    }
    
    [Fact]
    public void ChangeActivityType_GivenDoneTicketStatus_ShouldThrowTicketHasNoStatusToChangeActivityException()
    {
        //arrange
        var ticket = TicketsFactory.GetAll(state: State.Done());

        //act
        var exception = Record.Exception(() => ticket.ChangeActivityType(Guid.NewGuid()));
        
        //assert
        exception.ShouldBeOfType<TicketHasNoStatusToChangeActivityException>();
    }

    [Fact]
    public void ChangeState_GivenStateForChanges_ShouldChangeState()
    {
        //arrange
        var ticket = TicketsFactory.GetAll(state: State.New());
        var now = DateTime.Now;
        var state = State.InProgress();
        
        //act
        ticket.ChangeState(state, now);
        
        //assert
        ticket.State.Value.ShouldBe(state);
        ticket.State.ChangeDate.ShouldBe(now);    
    }

    [Fact]
    public void ChangeState_GivenStateNotForChanges_ShouldNotChangeState()
    {
        //arrange
        var state = State.Cancelled();
        var ticket = TicketsFactory.GetAll(state: state);
        var now = DateTime.Now;
        
        //act
        ticket.ChangeState(State.Done(), now);
        
        //assert
        ticket.State.Value.ShouldBe(state);
        ticket.State.ChangeDate.ShouldBe(now);  
    }
}