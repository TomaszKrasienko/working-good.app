using Shouldly;
using wg.modules.notifications.core.Providers;
using wg.modules.notifications.core.Providers.Abstractions;
using Xunit;

namespace wg.modules.notifications.core.tests.Providers;

public sealed class EmailNotificationProviderTests
{
    [Fact]
    public void GetForNewTicket_GivenAllNotNullOrEmptyArguments_ShouldReturnEmailNotificationWithFilledContent()
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
        result.Recipient.ShouldBe(recipient);
        result.Subject.ShouldBe($"#{ticketNumber} - {subject}");
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
    
    #region arrange
    private readonly IEmailNotificationProvider _emailNotificationProvider;

    public EmailNotificationProviderTests()
    {
        _emailNotificationProvider = new EmailNotificationProvider();
    }
    #endregion
}