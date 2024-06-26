using Shouldly;
using wg.modules.activities.domain.Entities;
using wg.modules.activities.domain.Exceptions;
using wg.tests.shared.Factories.Activities;
using wg.tests.shared.Helpers;
using Xunit;

namespace wg.modules.activities.domain.tests.Entities;

public sealed class InternalActivityCreateTests
{
    [Fact]
    public void Create_GivenValidArguments_ShouldReturnInternalActivity()
    {
        //arrange
        var id = Guid.NewGuid();
        var content = "My test content";
        var ticketId = Guid.NewGuid();
        var timeFrom = ShortDateTimeProvider.Get(DateTime.Now);
        var timeTo = ShortDateTimeProvider.Get(DateTime.Now.AddHours(1));
        
        //act
        var result = InternalActivity.Create(id, content, ticketId, timeFrom, timeTo);
        
        //assert
        result.Id.Value.ShouldBe(id);
        result.Content.Value.ShouldBe(content);
        result.TicketId.Value.ShouldBe(ticketId);
        result.ActivityTime.TimeFrom.ShouldBe(timeFrom);
        result.ActivityTime.TimeTo.ShouldBe(timeTo);
    }

    [Fact]
    public void Create_GivenEmptyContent_ShouldThrowEmptyActivityContentException()
    {
        //act
        var exception = Record.Exception(() => InternalActivity.Create(Guid.NewGuid(), string.Empty, Guid.NewGuid(),
            DateTime.Now, null));
        
        //assert
        exception.ShouldBeOfType<EmptyActivityContentException>();
    }
    
    [Fact]
    public void Create_GivenDateTimeToEarlierThanTimeFrom_ShouldThrowTimeToCanNotBeEarlierThanTimeFromException()
    {
        //act
        var exception = Record.Exception(() => InternalActivity.Create(Guid.NewGuid(), "Test", Guid.NewGuid(),
            DateTime.Now.AddHours(1), DateTime.Now));
        
        //assert
        exception.ShouldBeOfType<TimeToCanNotBeEarlierThanTimeFromException>();
    }
    
    [Fact]
    public void Create_GivenDatesFromOtherDays_ShouldThrowDatesCanNotBeFromOtherDaysException()
    {
        //act
        var exception = Record.Exception(() => InternalActivity.Create(Guid.NewGuid(), "Test", Guid.NewGuid(),
            DateTime.Now.AddDays(-1), DateTime.Now));
        
        //assert
        exception.ShouldBeOfType<DatesCanNotBeFromOtherDaysException>();
    }
    
    [Fact]
    public void Create_GivenPaidActivity_ShouldReturnInternalActivity()
    {
        //arrange
        var paidActivity = ActivityFactory.GetPaidActivity(DateTime.Now, DateTime.Now.AddHours(1));
        
        //act
        var result = InternalActivity.Create(paidActivity);
        
        //assert
        result.Id.ShouldBe(paidActivity.Id);
        result.Content.ShouldBe(paidActivity.Content);
        result.TicketId.ShouldBe(paidActivity.TicketId);
        result.ActivityTime.ShouldBe(paidActivity.ActivityTime);
    }
}                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                       