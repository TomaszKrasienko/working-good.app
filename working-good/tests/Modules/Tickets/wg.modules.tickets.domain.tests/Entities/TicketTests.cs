using System.Threading.RateLimiting;
using Shouldly;
using wg.modules.owner.domain.ValueObjects.User;
using wg.modules.tickets.domain.Entities;
using wg.modules.tickets.domain.Exceptions;
using wg.modules.tickets.domain.ValueObjects.Ticket;
using wg.tests.shared.Factories.Tickets;
using Xunit;

namespace wg.modules.tickets.domain.tests.Entities;

public sealed class TicketTests
{ 
    [Fact]
     public void ChangeState_GivenValidStatusForAvailableForChangesStatus_ShouldChangeState()
     {
         //arrange
         var ticket = TicketsFactory.Get();
         var status = Status.Open();
         var now = DateTime.Now;
         
         
         //act
         ticket.ChangeStatus(status, now);
         
         //assert
         ticket.Status.Value.ShouldBe(status);
         ticket.Status.ChangeDate.ShouldBe(now);    
     }

     [Fact]
     public void ChangeStatus_GivenStatusNotForChanges_ShouldNotChangeStatus()
     {
         //arrange
         var ticket = TicketsFactory.Get();
         ticket.ChangeStatus(Status.Done(), DateTime.Now);
         var now = DateTime.Now;
         
         //act
         ticket.ChangeStatus(Status.Open(), now);
         
         //assert
         ticket.Status.Value.ShouldBe(Status.Done());
         ticket.Status.ChangeDate.ShouldNotBe(now);  
     }

     [Fact]
     public void ChangeStatus_GivenEmptyStatus_ShouldThrowEmptyStatusException()
     {
         //arrange
         var ticket = TicketsFactory.Get();
         
         //act
         var exception = Record.Exception(() => ticket.ChangeStatus(string.Empty, DateTime.Now));
         
         //assert
         exception.ShouldBeOfType<EmptyStatusException>();
     }

     [Fact]
     public void ChangeStatus_GivenNotAvailableStatus_ShouldThrowUnavailableStatusException()
     {
         //arrange
         var ticket = TicketsFactory.Get();
         
         //act
         var exception = Record.Exception(() => ticket.ChangeStatus("State", DateTime.Now));
         
         //assert
         exception.ShouldBeOfType<UnavailableStatusException>();
     }

     [Fact]
     public void ChangeAssignedEmployee_GivenStatusForChanges_ShouldChangeAssignedEmployee()
     {
         //arrange
         var ticket = TicketsFactory.Get();
         var substituteEmployeeId = Guid.NewGuid();
         
         //act
         ticket.ChangeAssignedEmployee(substituteEmployeeId);
         
         //assert
         ticket.AssignedEmployee.Value.ShouldBe(substituteEmployeeId);
     }
     
     [Fact]
     public void ChangeAssignedEmployee_GivenStatusNotForChanges_ShouldNotChangeAssignedEmployee()
     {
         //arrange
         var ticket = TicketsFactory.Get();
         var oldAssignedEmployee = Guid.NewGuid();
         ticket.ChangeAssignedEmployee(oldAssignedEmployee);
         ticket.ChangeStatus(Status.Done(), DateTime.Now);
         
         //act
         ticket.ChangeAssignedEmployee(Guid.NewGuid());
         
         //assert
         ticket.AssignedEmployee.Value.ShouldBe(oldAssignedEmployee);
     }
//
//     [Fact]
//     public void ChangeAssignedUser_GivenUserIdAndDateForStateNew_ShouldChangeAssignedUserAndStateToOpenAndStateDate()
//     {
//         //arrange
//         var ticket = TicketsFactory.GetOnlyRequired(state: State.New());
//         var userId = Guid.NewGuid();
//         var date = DateTime.Now;
//         
//         //act
//         ticket.ChangeAssignedUser(userId, date);
//         
//         //assert
//         ticket.AssignedUser.Value.ShouldBe(userId);
//         ticket.State.Value.ShouldBe(State.Open());
//         ticket.State.ChangeDate.ShouldBe(date);
//     }
//     
//     [Fact]
//     public void ChangeAssignedUser_GivenUserIdAndDateForStateInProgress_ShouldChangeAssignedUserAndNotChangeState()
//     {
//         //arrange
//         var ticket = TicketsFactory.GetOnlyRequired(state: State.InProgress());
//         var currentStateDate = ticket.State.ChangeDate;
//         var userId = Guid.NewGuid();
//         var date = DateTime.Now;
//         
//         //act
//         ticket.ChangeAssignedUser(userId, date);
//         
//         //assert
//         ticket.AssignedUser.Value.ShouldBe(userId);
//         ticket.State.Value.ShouldBe(State.InProgress());
//         ticket.State.ChangeDate.ShouldBe(currentStateDate);
//     }
//
//     [Fact]
//     public void ChangeAssignedUser_ForCancelledState_ShouldNotChangeState()
//     {        
//         //arrange
//         var ticket = TicketsFactory.GetOnlyRequired(state: State.Cancelled());
//         var originalState = ticket.State;
//         var userId = Guid.NewGuid();
//         var date = DateTime.Now;
//         
//         //act
//         ticket.ChangeAssignedUser(userId, date);
//         
//         //assert
//         ticket.State.ShouldBe(originalState);
//     }
//     
//     [Fact]
//     public void ChangeAssignedEmployee_GivenEmployeeIddForStateInProgress_ShouldChangeAssignedEmployee()
//     {
//         //arrange
//         var ticket = TicketsFactory.GetOnlyRequired(state: State.InProgress());
//         var employeeId = Guid.NewGuid();
//         
//         //act
//         ticket.ChangeAssignedEmployee(employeeId);
//         
//         //assert
//         ticket.AssignedEmployee.Value.ShouldBe(employeeId);
//     }
//
//     [Fact]
//     public void ChangeAssignedUser_ForCancelledState_ShouldNotChangeAssigning()
//     {        
//         //arrange
//         var ticket = TicketsFactory.GetOnlyRequired(state: State.Cancelled());
//         var originalEmployee = ticket.AssignedEmployee;
//         var employeeId = Guid.NewGuid();
//         
//         //act
//         ticket.ChangeAssignedEmployee(employeeId);
//         
//         //assert
//         ticket.AssignedEmployee.ShouldBe(originalEmployee);
//     }
//
//     [Fact]
//     public void RemoveAssignedUser_ForOpenState_ShouldRemoveAssignedUser()
//     {
//         //arrange
//         var ticket = TicketsFactory.GetAll(state: State.Open());
//         
//         //act
//         ticket.RemoveAssignedUser();
//         
//         //assert
//         ticket.AssignedUser.ShouldBeNull();
//     }
//     
//     [Fact]
//     public void RemoveAssignedUser_ForCancelledState_ShouldNotRemoveAssignedUser()
//     {
//         //arrange
//         var ticket = TicketsFactory.GetAll(state: State.Open());
//         var userId = ticket.AssignedUser;
//         ticket.ChangeState(State.Cancelled(), DateTime.Now);
//         
//         //act
//         ticket.RemoveAssignedUser();
//         
//         //assert
//         ticket.AssignedUser.Value.ShouldBe(userId.Value);
//     }
//     
//     [Fact]
//     public void ChangeProject_GivenProjectId_ChangeProjectId()
//     {        
//         //arrange
//         var ticket = TicketsFactory.GetOnlyRequired(state: State.Cancelled());
//         var projectId = Guid.NewGuid();
//         
//         //act
//         ticket.ChangeProject(projectId);
//         
//         //assert
//         ticket.ProjectId.Value.ShouldBe(projectId);
//     }
//     
//     [Fact]
//     public void AddMessage_GivenMessageAnd_ShouldAddToMessages()
//     {
//         //arrange
//         var ticket = TicketsFactory.GetOnlyRequired(state: State.New());
//         var id = Guid.NewGuid();
//         var sender = "joe@doe.pl";
//         var subject = "Test subject";
//         var content = "Test content";
//         var createdAt = DateTime.Now;
//         
//         //act
//         ticket.AddMessage(id, sender, subject, content, createdAt);
//         
//         //assert
//         var message = ticket.Messages.FirstOrDefault(x => x.Id.Equals(id));
//         ticket.State.Value.ShouldBe(State.WaitingForResponse());
//         message.ShouldNotBeNull();
//         message.Id.Value.ShouldBe(id);
//         message.Sender.Value.ShouldBe(sender);
//         message.Subject.Value.ShouldBe(subject);
//         message.Content.Value.ShouldBe(content);
//         message.CreatedAt.Value.ShouldBe(createdAt);
//     }
//

}