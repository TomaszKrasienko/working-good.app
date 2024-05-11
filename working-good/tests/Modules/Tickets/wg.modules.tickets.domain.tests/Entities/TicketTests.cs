using Shouldly;
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

     [Fact]
     public void ChangeAssignedUser_GivenStatusForChanges_ShouldChangeAssignedUser()
     {
         //arrange
         var ticket = TicketsFactory.Get();
         var userId = Guid.NewGuid();
         
         //act
         ticket.ChangeAssignedUser(userId);
         
         //assert
         ticket.AssignedUser.Value.ShouldBe(userId);
     }
     
     [Fact]
     public void ChangeAssignedUser_GivenStatusNotForChanges_ShouldNotChangeAssignedUser()
     {
         //arrange
         var ticket = TicketsFactory.Get();
         var oldUserId = Guid.NewGuid();
         ticket.ChangeAssignedUser(oldUserId);
         ticket.ChangeStatus(Status.Cancelled(), DateTime.Now);
         
         //act
         ticket.ChangeAssignedUser(oldUserId);
         
         //assert
         ticket.AssignedUser.Value.ShouldBe(oldUserId);
     }

     [Fact]
     public void RemoveAssignedUser_GivenStatusForChanges_ShouldRemoveAssignedUser()
     {
         //arrange
         var ticket = TicketsFactory.Get();
         ticket.ChangeAssignedUser(Guid.NewGuid());
         
         //act
         ticket.RemoveAssignedUser();
         
         //assert
         ticket.AssignedUser.ShouldBeNull();
     }
     
     [Fact]
     public void RemoveAssignedUser_GivenStatusNotForChanges_ShouldNotRemoveAssignedUser()
     {
         //arrange
         var ticket = TicketsFactory.Get();
         var oldUserId = Guid.NewGuid();
         ticket.ChangeAssignedUser(oldUserId);
         ticket.ChangeStatus(Status.Done(), DateTime.Now);
         
         //act
         ticket.RemoveAssignedUser();
         
         //assert
         ticket.AssignedUser.Value.ShouldBe(oldUserId);
     }
     
     [Fact]
     public void ChangeProject_GivenStatusForChanges_ShouldChangeProjectId()
     {        
         //arrange
         var ticket = TicketsFactory.Get();
         var projectId = Guid.NewGuid();
         
         //act
         ticket.ChangeProject(projectId);
         
         //assert
         ticket.ProjectId.Value.ShouldBe(projectId);
     }
     
     [Fact]
     public void ChangeProject_GivenStatusNotForChanges_ShouldNotChangeProjectId()
     {        
         //arrange
         var ticket = TicketsFactory.Get();
         var oldProjectId = Guid.NewGuid();
         ticket.ChangeProject(oldProjectId);
         ticket.ChangeStatus(Status.Cancelled(), DateTime.Now);
         
         //act
         ticket.ChangeProject(Guid.NewGuid());
         
         //assert
         ticket.ProjectId.Value.ShouldBe(oldProjectId);
     }
     
     [Fact]
     public void AddMessage_GivenMessageFromUser_ShouldAddToMessagesAndChangeStatusToWaitingForResponse()
     {
         //arrange
         var ticket = TicketsFactory.Get();
         var id = Guid.NewGuid();
         var sender = "joe@doe.pl";
         var subject = "Test subject";
         var content = "Test content";
         var createdAt = DateTime.Now;
         
         //act
         ticket.AddMessage(id, sender, subject, content, createdAt, true);
         
         //assert
         var message = ticket.Messages.FirstOrDefault(x => x.Id.Equals(id));
         ticket.Status.Value.ShouldBe(Status.WaitingForResponse());
         message.ShouldNotBeNull();
         message.Id.Value.ShouldBe(id);
         message.Sender.Value.ShouldBe(sender);
         message.Subject.Value.ShouldBe(subject);
         message.Content.Value.ShouldBe(content);
         message.CreatedAt.Value.ShouldBe(createdAt);
     }
     
     [Fact]
     public void AddMessage_GivenMessageNotFromUser_ShouldAddToMessagesAndChangeStatusToCustomerReplied()
     {
         //arrange
         var ticket = TicketsFactory.Get();
         ticket.ChangeStatus(Status.Done(), DateTime.Now);
         var id = Guid.NewGuid();
         var sender = "joe@doe.pl";
         var subject = "Test subject";
         var content = "Test content";
         var createdAt = DateTime.Now;
         
         //act
         ticket.AddMessage(id, sender, subject, content, createdAt, false);
         
         //assert
         var message = ticket.Messages.FirstOrDefault(x => x.Id.Equals(id));
         ticket.Status.Value.ShouldBe(Status.CustomerReplied());
         message.ShouldNotBeNull();
         message.Id.Value.ShouldBe(id);
         message.Sender.Value.ShouldBe(sender);
         message.Subject.Value.ShouldBe(subject);
         message.Content.Value.ShouldBe(content);
         message.CreatedAt.Value.ShouldBe(createdAt);
     }
}