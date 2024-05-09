using Shouldly;
using wg.modules.activities.infrastructure.Queries.Mappers;
using wg.tests.shared.Factories.Activities;
using Xunit;

namespace wg.modules.activities.infrastructure.tests.Queries.Mappers;

public sealed class ExtensionsTests
{
    [Fact]
    public void AsDto_GivenActivityWithoutTimeTo_ShouldReturnActivityDto()
    {
        //arrange
        var activity = ActivityFactory.GetRandom(DateTime.Now, null);
        
        //act
        var result = activity.AsDto();
        
        //assert
        result.Id.ShouldBe(activity.Id.Value);
        result.Content.ShouldBe(activity.Content.Value);
        result.TimeFrom.ShouldBe(activity.ActivityTime.TimeFrom);
        result.TimeTo.ShouldBeNull();
        result.Summary.ShouldBe(TimeSpan.Zero);
    }
    
    [Fact]
    public void AsDto_GivenActivityWithTimeTo_ShouldReturnActivityDto()
    {
        //arrange
        var activity = ActivityFactory.GetRandom(DateTime.Now.AddHours(-2), DateTime.Now);
        
        //act
        var result = activity.AsDto();
        
        //assert
        result.Id.ShouldBe(activity.Id.Value);
        result.Content.ShouldBe(activity.Content.Value);
        result.TimeFrom.ShouldBe(activity.ActivityTime.TimeFrom);
        result.TimeTo.ShouldBe(activity.ActivityTime.TimeTo);
        result.Summary.ShouldBe(activity.ActivityTime.Summary);
    }
}