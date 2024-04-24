using Shouldly;
using wg.modules.activities.domain.Entities;
using Xunit;

namespace wg.modules.activities.domain.tests.Entities;

public sealed class DailyEmployeeActivitiesCreateTests
{
    [Fact]
    public void Create_GivenValidArguments_ShouldReturnDailyEmployeeActivities()
    {
        //arrange
        var id = Guid.NewGuid();
        var day = DateTime.Now;
        var userId = Guid.NewGuid();
        
        //act
        var result = DailyEmployeeActivities.Create(id, day, userId);
        
        //assert
        result.Id.Value.ShouldBe(id);
        result.Day.Value.Date.ShouldBe(day.Date);
        result.Day.Value.Hour.ShouldBe(0);
        result.Day.Value.Minute.ShouldBe(0);
        result.Day.Value.Second.ShouldBe(0);
        result.UserId.Value.ShouldBe(userId);
    }
}