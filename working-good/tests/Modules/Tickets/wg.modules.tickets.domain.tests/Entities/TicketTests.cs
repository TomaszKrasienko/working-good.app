using Org.BouncyCastle.Asn1.X509;
using Shouldly;
using wg.modules.tickets.domain.Entities;
using wg.modules.tickets.domain.Exceptions;
using wg.modules.tickets.domain.ValueObjects.Ticket;
using wg.tests.shared.Factories.Tickets;
using Xunit;

namespace wg.modules.tickets.domain.tests.Entities;

public sealed class TicketTests
{
    [Fact]
    public void ChangeSubject_GivenSubject_ShouldChangeSubject()
    {
        //arrange
        var ticket = TicketsFactory.Get();
        var newSubject = "New subject";
        
        //act
        ticket.ChangeSubject(newSubject);
        
        //assert
        ticket.Subject.Value.ShouldBe(newSubject);
    }

    [Fact]
    public void ChangeSubject_GivenEmptySubject_ShouldThrowEmptySubjectException()
    {
        //arrange
        var ticket = TicketsFactory.Get();
        
        //act
        var exception = Record.Exception(() => ticket.ChangeSubject(string.Empty));
        
        //assert
        exception.ShouldBeOfType<EmptySubjectException>();
    }
    
    [Fact]
    public void ChangeContent_GivenContent_ShouldChangeContent()
    {
        //arrange
        var ticket = TicketsFactory.Get();
        var newContent = "New content";
        
        //act
        ticket.ChangeContent(newContent);
        
        //assert
        ticket.Content.Value.ShouldBe(newContent);
    }

    [Fact]
    public void ChangeContent_GivenEmptySubject_ShouldThrowEmptyContentException()
    {
        //arrange
        var ticket = TicketsFactory.Get();
        
        //act
        var exception = Record.Exception(() => ticket.ChangeContent(string.Empty));
        
        //assert
        exception.ShouldBeOfType<EmptyContentException>();
    }
    
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
     public void ChangePriority_GivenPriorityAsTrueAndNullEmployee_ShouldThrowMissingAssignedEmployeeException()
     {
         //arrange
         var ticket = TicketsFactory.Get();
         
         //act
         var exception = Record.Exception(() => ticket.ChangePriority(true, TimeSpan.FromHours(15)));
         
         //assert
         exception.ShouldBeOfType<MissingAssignedEmployeeException>();
     }

     [Fact]
     public void ChangePriority_GivenPriorityAsTrueWithAssignedEmployeeAndValidSlaTimeAndTicketWithoutExpirationDate_ShouldChangePriorityAndExpirationDate()
     {
         //arrange
         var ticket = TicketsFactory.Get();
         ticket.ChangeAssignedEmployee(Guid.NewGuid());
         var timeSpan = TimeSpan.FromHours(10);
         
         //act
         ticket.ChangePriority(true, timeSpan);
         
         //assert
         ticket.IsPriority.Value.ShouldBeTrue();
         ticket.ExpirationDate.Value.ShouldBe(ticket.CreatedAt.Value.Add(timeSpan));
     }
     
     [Fact]
     public void ChangePriority_GivenPriorityAsTrueWithAssignedEmployeeAndValidSlaTimeAndTicketWithBiggerExpirationDate_ShouldChangePriorityAndExpirationDate()
     {
         //arrange
         var ticket = TicketsFactory.Get();
         ticket.ChangeAssignedEmployee(Guid.NewGuid());
         ticket.ChangeExpirationDate(DateTime.Now.AddDays(10));
         var timeSpan = TimeSpan.FromHours(10);
         
         //act
         ticket.ChangePriority(true, timeSpan);
         
         //assert
         ticket.IsPriority.Value.ShouldBeTrue();
         ticket.ExpirationDate.Value.ShouldBe(ticket.CreatedAt.Value.Add(timeSpan));
     }
    
     [Fact]
     public void ChangePriority_GivenPriorityAsTrueWithAssignedEmployeeAndValidSlaTimeAndTicketWithSmallerExpirationDate_ShouldChangePriorityAndWithoutExpirationDate()
     {
         //arrange
         var ticket = TicketsFactory.Get();
         ticket.ChangeAssignedEmployee(Guid.NewGuid());
         var expirationDate = ticket.CreatedAt.Value.Add(TimeSpan.FromMinutes(30));
         ticket.ChangeExpirationDate(expirationDate);
         var timeSpan = TimeSpan.FromHours(10);
         
         //act
         ticket.ChangePriority(true, timeSpan);
         
         //assert
         ticket.IsPriority.Value.ShouldBeTrue();
         ticket.ExpirationDate.Value.ShouldBe(expirationDate);
     }

     [Fact]
     public void ChangePriority_GivenPriorityAsFalse_ShouldChangePriority()
     {
         //arrange
         var ticket = TicketsFactory.Get();
         
         //act
         ticket.ChangePriority(false);
         
         //assert
         ticket.IsPriority.Value.ShouldBeFalse();
     }
     
     [Fact]
     public void ChangePriority_GivenPriorityAsTrueAndNullSlaTime_ShouldThrowInvalidSlaTimeForAssignedEmployeeException()
     {
         //arrange
         var ticket = TicketsFactory.Get();
         ticket.ChangeAssignedEmployee(Guid.NewGuid());
         
         //act
         var exception = Record.Exception(() => ticket.ChangePriority(true, null));
         
         //assert
         exception.ShouldBeOfType<InvalidSlaTimeForTicketException>();
     }
     
     [Fact]
     public void ChangePriority_GivenPriorityAsTrueAndSlaTimeBelowZero_ShouldThrowInvalidSlaTimeForAssignedEmployeeException()
     {
         //arrange
         var ticket = TicketsFactory.Get();
         ticket.ChangeAssignedEmployee(Guid.NewGuid());
         
         //act
         var exception = Record.Exception(() => ticket.ChangePriority(true, TimeSpan.FromHours(-1)));
         
         //assert
         exception.ShouldBeOfType<InvalidSlaTimeForTicketException>();
     }

     [Fact]
     public void ChangeExpirationDate_GivenNotPriorityTicketAndDateNotBeforeNowDate_ShouldChangeExpirationDate()
     {
        //arrange
        var ticket = TicketsFactory.Get();
        var expirationDate = DateTime.Now.AddDays(2);

        //act
        ticket.ChangeExpirationDate(expirationDate);
        
        //assert
        ticket.ExpirationDate.Value.ShouldBe(expirationDate);
     }

     [Fact]
     public void ChangeExpirationDate_GivenPriorityTicketAndNotNullLimitedTime_ShouldChangeExpirationDate()
     {
         //arrange
         var ticket = TicketsFactory.Get();
         ticket.ChangeAssignedEmployee(Guid.NewGuid());
         ticket.ChangePriority(true, TimeSpan.FromDays(2));
         var expirationDate = DateTime.Now.AddDays(2);
         var limitTime = TimeSpan.FromDays(3);

         //act
         ticket.ChangeExpirationDate(expirationDate, limitTime);
        
         //assert
         ticket.ExpirationDate.Value.ShouldBe(expirationDate);
     }

     [Fact]
     public void ChangeExpirationDate_GivenPriorityTicketAndNullLimitedTime_ShouldThrowNullLimitTimeException()
     {
         //arrange
         var ticket = TicketsFactory.Get();
         ticket.ChangeAssignedEmployee(Guid.NewGuid());
         ticket.ChangePriority(true, TimeSpan.FromDays(2));
         var expirationDate = DateTime.Now.AddDays(2);
         
         //act
         var exception = Record.Exception(() => ticket.ChangeExpirationDate(expirationDate, null));
         
         //assert
         exception.ShouldBeOfType<NullLimitTimeException>();
     }

     [Fact]
     public void ChangeExpirationDate_GivenPriorityTicketAndTooLateExpirationDate_ShouldThrowExpirationDateTooLateException()
     {
         //arrange
         var ticket = TicketsFactory.Get();
         ticket.ChangeAssignedEmployee(Guid.NewGuid());
         ticket.ChangePriority(true, TimeSpan.FromHours(2));
         var limitTime = TimeSpan.FromDays(1);
         var expirationDate = DateTime.Now.AddDays(4);
         
         //act
         var exception = Record.Exception(() => ticket.ChangeExpirationDate(expirationDate, limitTime));
         
         //assert
         exception.ShouldBeOfType<ExpirationDateTooLateException>();
     }
     
     [Fact]
     public void AddMessage_GivenMessageFromUser_ShouldAddToMessagesAndChangeStatusToWaitingForResponse()
     {
         //arrange
         var ticket = TicketsFactory.Get();
         ticket.ChangeAssignedEmployee(Guid.NewGuid());
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
         ticket.ChangeStatus(Status.Open(), DateTime.Now);
         ticket.ChangeAssignedEmployee(Guid.NewGuid());
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

     [Fact]
     public void AddMessage_GivenMessageFromUserWithoutAssignedEmployee_ShouldThrowCanNotAddMessageWithoutAssignedEmployeeException()
     {
         //arrange
         var ticket = TicketsFactory.Get();
         ticket.ChangeStatus(Status.Done(), DateTime.Now);

         //act
         var exception = Record.Exception(() => ticket.AddMessage(Guid.NewGuid(), "test@test.pl", "My subject",
             "My content", DateTime.Now, false));
         
         //assert
         exception.ShouldBeOfType<CanNotAddMessageWithoutAssignedEmployeeException>();
     }
}