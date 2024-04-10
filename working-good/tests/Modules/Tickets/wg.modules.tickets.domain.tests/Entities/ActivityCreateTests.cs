using Shouldly;
using wg.modules.tickets.domain.Entities;
using wg.modules.tickets.domain.Exceptions;
using Xunit;

namespace wg.modules.tickets.domain.tests.Entities;

public sealed class ActivityCreateTests
{
    [Fact]
    public void Create_GivenAllArguments_ShouldReturnActivityWithFilledFields()
    {
        //arrange
        var id = Guid.NewGuid();
        var now = new DateTime(2024, 4, 10, 10, 10, 0);
        var timeFrom = now.AddHours(-1);
        var timeTo = now.AddHours(1);
        var note = "Test note";
        var isPaid = true;
        var userId = Guid.NewGuid();
        
        //act
        var result = Activity.Create(id, timeFrom, timeTo, note, isPaid, userId);
        
        //assert
        result.ShouldNotBeNull();
        result.Id.Value.ShouldBe(id);
        result.ActivityTime.TimeFrom.ShouldBe(timeFrom);
        result.ActivityTime.TimeTo.ShouldBe(timeTo);
        result.ActivityTime.Summary.ShouldBe(TimeSpan.FromHours(2));
        result.Note.Value.ShouldBe(note);
        result.IsPaid.Value.ShouldBeTrue();
    }
    
    [Fact]
    public void Create_GivenWithoutTimeTo_ShouldReturnActivityWithFilledFields()
    {
        //arrange
        var id = Guid.NewGuid();
        var timeFrom = DateTime.Now.AddHours(-1);
        var note = "Test note";
        var isPaid = true;
        var userId = Guid.NewGuid();
        
        //act
        var result = Activity.Create(id, timeFrom, note, isPaid, userId);
        
        //assert
        result.ShouldNotBeNull();
        result.Id.Value.ShouldBe(id);
        result.ActivityTime.TimeFrom.ShouldBe(timeFrom);
        result.ActivityTime.TimeTo.ShouldBeNull();        
        result.ActivityTime.Summary.ShouldBe(TimeSpan.Zero);
        result.Note.Value.ShouldBe(note);
        result.IsPaid.Value.ShouldBeTrue();
    }

    [Fact]
    public void Create_GivenEmptyNote_ShouldThrowEmptyNoteException()
    {
        //act
        var exception = Record.Exception(() => Activity.Create(Guid.NewGuid(), DateTime.Now.AddHours(-1), 
            DateTime.Now.AddHours(1), string.Empty, true, Guid.NewGuid()));
        
        //assert
        exception.ShouldBeOfType<EmptyNoteException>();
    }

    [Fact]
    public void Create_GivenTimeToBeforeTimeFrom_ShouldThrowTimeToCanNotBeEarlierThanTimeFromException()
    {
        //act
        var exception = Record.Exception(() => Activity.Create(Guid.NewGuid(), DateTime.Now.AddHours(1), 
            DateTime.Now.AddHours(-1), "Test note", true, Guid.NewGuid()));
        
        //assert
        exception.ShouldBeOfType<TimeToCanNotBeEarlierThanTimeFromException>();
    }
}