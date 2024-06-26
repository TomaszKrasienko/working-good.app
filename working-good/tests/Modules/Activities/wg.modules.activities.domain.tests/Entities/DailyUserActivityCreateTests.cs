using Shouldly;
using wg.modules.activities.domain.Entities;
using Xunit;

namespace wg.modules.activities.domain.tests.Entities;

public sealed class DailyUserActivityCreateTests
{
    [Fact]
    public void Create_GivenValidArguments_ShouldReturnDailyUserActivity()
    {
        //arrange
        var day = DateTime.Now;
        var userId = Guid.NewGuid();
        
        //act
        var result = DailyUserActivity.Create(day, userId);
        
        //assert
        result.Day.Value.Date.ShouldBe(day.Date);
        result.Day.Value.Hour.ShouldBe(0);
        result.Day.Value.Minute.ShouldBe(0);
        result.Day.Value.Second.ShouldBe(0);
        result.UserId.Value.ShouldBe(userId);
    }
}