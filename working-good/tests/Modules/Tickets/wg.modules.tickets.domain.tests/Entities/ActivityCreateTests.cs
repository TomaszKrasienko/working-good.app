using Shouldly;
using wg.modules.tickets.domain.Entities;
using Xunit;

namespace wg.modules.tickets.domain.tests.Entities;

public sealed class ActivityCreateTests
{
    [Fact]
    public void Create_GivenAllArguments_ShouldReturnActivityWithFilledFields()
    {
        //arrange
        var id = Guid.NewGuid();
        var timeFrom = DateTime.Now.AddHours(-1);
        var timeTo = DateTime.Now.AddHours(1);
        var note = "Test note";
        var isPaid = true;
        
        //act
        var result = Activity.Create(id, timeFrom, timeTo, note, isPaid);
        
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
        
        //act
        var result = Activity.Create(id, timeFrom, note, isPaid);
        
        //assert
        result.ShouldNotBeNull();
        result.Id.Value.ShouldBe(id);
        result.ActivityTime.TimeFrom.ShouldBe(timeFrom);
        result.ActivityTime.TimeTo.ShouldBeNull();        
        result.ActivityTime.Summary.ShouldBe(TimeSpan.Zero);
        result.Note.Value.ShouldBe(note);
        result.IsPaid.Value.ShouldBeTrue();
    }
}