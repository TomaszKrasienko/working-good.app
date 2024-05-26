using Shouldly;
using wg.modules.notifications.core.Providers;
using wg.modules.notifications.core.Providers.Abstractions;
using Xunit;

namespace wg.modules.notifications.core.tests.Providers;

public sealed class EmailNotificationProviderTests
{
    [Fact]
    public void GetForNewTicket_GivenAllNotNullOrEmptyArguments_ShouldReturnEmailNotification()
    {
        //arrange
        var recipient = "test@test.pl";
        var ticketNumber = 1;
        var content = "Test content";
        var subject = "Test subject";
        
        //act
        var result = _emailNotificationProvider.GetForNewTicket(recipient,
            ticketNumber, content, subject);
        
        //assert
        result.ShouldNotBeNull();
        result.Recipient[0].ShouldBe(recipient);
        result.Subject.ShouldBe($"Ticket number #{ticketNumber} - {subject}");
        result.Content.ShouldBe($"A ticket has been created to which you have been assigned with the following content\n{content}");
    }

    [Fact]
    public void GetForNewTicket_GivenEmptyRecipient_ShouldReturnNull()
    {
        //act
        var result = _emailNotificationProvider.GetForNewTicket(string.Empty,
            1, "Test content", "Test subject");
        
        //assert
        result.ShouldBeNull();
    }
    
    [Fact]
    public void GetForNewTicket_GivenZeroNumber_ShouldReturnNull()
    {
        //act
        var result = _emailNotificationProvider.GetForNewTicket("test@test.pl",
            0, "Test content", "Test subject");
        
        //assert
        result.ShouldBeNull();
    }
    
    [Fact]
    public void GetForNewTicket_GivenEmptyContent_ShouldReturnNull()
    {
        //act
        var result = _emailNotificationProvider.GetForNewTicket("test@test.pl",
            1, string.Empty, "Test subject");
        
        //assert
        result.ShouldBeNull();
    }

    [Fact]
    public void GetForNewTicket_GivenEmptySubject_ShouldReturnNull()
    {
        //act
        var result = _emailNotificationProvider.GetForNewTicket("test@test.pl",
            1, "Test content", string.Empty);
        
        //assert
        result.ShouldBeNull();
    }

    [Fact]
    public void GetForNewUser_GivenAllNotNullOrEmptyArguments_ShouldReturnEmailNotification()
    {
        //arrange
        var recipient = "test@test.pl";
        var firstName = "Joe";
        var lastName = "Doe";
        var verificationToken = Guid.NewGuid().ToString();
        
        //act
        var result = _emailNotificationProvider.GetForNewUser(recipient, firstName, lastName, verificationToken);
        
        //assert
        result.ShouldNotBeNull();
        result.Recipient[0].ShouldBe(recipient);
        result.Subject.ShouldBe("Thank you for registration");
        result.Content.ShouldContain(firstName);
        result.Content.ShouldContain(lastName);
        result.Content.ShouldContain(verificationToken);
    }
    
    [Fact]
    public void GetForNewUser_GivenEmptyRecipient_ShouldReturnNull()
    {
        //act
        var result = _emailNotificationProvider.GetForNewUser(string.Empty, "Joe", 
            "Doe", Guid.NewGuid().ToString());
        
        //assert
        result.ShouldBeNull();
    }
    
    [Fact]
    public void GetForNewUser_GivenEmptyFirstName_ShouldReturnNull()
    {
        //act
        var result = _emailNotificationProvider.GetForNewUser("test@test.pl", string.Empty, 
            "Doe", Guid.NewGuid().ToString());
        
        //assert
        result.ShouldBeNull();
    }
    
    [Fact]
    public void GetForNewUser_GivenEmptyLastName_ShouldReturnNull()
    {
        //act
        var result = _emailNotificationProvider.GetForNewUser("test@test.pl", "Joe", 
            string.Empty, Guid.NewGuid().ToString());
        
        //assert
        result.ShouldBeNull();
    }
    
    [Fact]
    public void GetForNewUser_GivenEmptyVerificationToken_ShouldReturnNull()
    {
        //act
        var result = _emailNotificationProvider.GetForNewUser("test@test.pl", "Joe", 
            "Doe", string.Empty);
        
        //assert
        result.ShouldBeNull();
    }

    [Fact]
    public void GetForAssigning_GivenRecipientAndTicketNumber_ShouldReturnEmailNotification()
    {
        //arrange
        var recipient = "test@test.pl";
        var ticketNumber = 123;
        
        //act
        var result = _emailNotificationProvider.GetForAssigning(recipient, ticketNumber);
        
        //assert
        result.Recipient[0].ShouldBe(recipient);
        result.Subject.ShouldBe($"Ticket with number: {ticketNumber} has been assigned to you");
        result.Content.ShouldBe($"Ticket with number: {ticketNumber} has been assigned to you");
    }
    
    #region arrange
    private readonly IEmailNotificationProvider _emailNotificationProvider;

    public EmailNotificationProviderTests()
    {
        _emailNotificationProvider = new EmailNotificationProvider();
    }
    #endregion
}