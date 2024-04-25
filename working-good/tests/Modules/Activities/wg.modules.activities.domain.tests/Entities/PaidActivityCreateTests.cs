using Shouldly;
using wg.modules.activities.domain.Entities;
using Xunit;

namespace wg.modules.activities.domain.tests.Entities;

public sealed class PaidActivityCreateTests
{
    [Fact]
    public void Create_GivenValidArguments_ShouldReturnPaidActivity()
    {
        //arrange
        var id = Guid.NewGuid();
        var content = "My test content";
        var ticketId = Guid.NewGuid();
        var timeFrom = DateTime.Now;
        var timeTo = DateTime.Now.AddHours(1);
        
        //act
        var result = PaidActivity.Create(id, content, ticketId, timeFrom, timeTo);
        
        //assert
        result.Id.Value.ShouldBe(id);
        result.Content.Value.ShouldBe(content);
        result.TicketId.Value.ShouldBe(ticketId);
        result.ActivityTime.TimeFrom.ShouldBe(timeFrom);
        result.ActivityTime.TimeTo.ShouldBe(timeTo);
    }
}