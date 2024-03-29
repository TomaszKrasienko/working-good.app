using Shouldly;
using wg.modules.messages.core.Entities;
using Xunit;

namespace wg.modules.messages.core.tests.Entities;

public sealed class ClientMessageCreateTests
{
    [Theory]
    [InlineData("Ticket number #{0} - Test subject")]
    [InlineData("PD: Ticket number #{0} - Test subject")]
    [InlineData("ODP: Ticket number #{0} - Test subject")]
    public void Create_GivenSubjectStartedFromNumber_ShouldReturnMessageWithFilledNumber(string subject)
    {
        //arrange
        var number = 14;
        var subjectWithNumber = string.Format(subject, number);
        var content = "Test content";
        var sender = "test@test.pl";
        var createdAt = DateTime.Now;
        var assignedEmployee = Guid.NewGuid();
        
        //act
        var message = ClientMessage.Create(subjectWithNumber, content, sender, createdAt, assignedEmployee);
        
        //assert
        message.Id.Value.ShouldNotBe(Guid.Empty);
        message.Subject.ShouldBe(subjectWithNumber);
        message.Content.ShouldBe(content);
        message.Sender.ShouldBe(sender);
        message.CreatedAt.ShouldBe(createdAt);
        message.Number.ShouldBe(number);
    }

    [Fact]
    public void Create_GivenSubjectWithoutNumber_ShouldReturnMessageWithNullNumber()
    {        
        //arrange
        var subject = "#Test subject";
        var content = "Test content";
        var sender = "test@test.pl";
        var createdAt = DateTime.Now;
        var assignedEmployee = Guid.NewGuid();
        
        //act
        var message = ClientMessage.Create(subject, content, sender, createdAt, assignedEmployee);
        
        //assert
        message.Id.Value.ShouldNotBe(Guid.Empty);
        message.Subject.ShouldBe(subject);
        message.Content.ShouldBe(content);
        message.Sender.ShouldBe(sender);
        message.CreatedAt.ShouldBe(createdAt);
        message.Number.ShouldBeNull();
    }
}