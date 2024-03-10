using Shouldly;
using wg.modules.companies.domain.Entities;
using wg.modules.companies.domain.Exceptions;
using Xunit;

namespace wg.modules.companies.domain.tests;

public sealed class ProjectCreateTests
{
    [Fact]
    public void Create_GivenAllValidArguments_ShouldReturnProjectWithFilledFields()
    {
        //arrange
        var id = Guid.NewGuid();
        var title = "Test project";
        var description = "My description of test project";
        var plannedStart = DateTime.Now.AddMonths(1);
        var plannedFinish = DateTime.Now.AddYears(1);
        
        //act
        var result = Project.Create(id, title, description, plannedStart, plannedFinish);
        
        //assert
        result.ShouldNotBeNull();
        result.Id.Value.ShouldBe(id);
        result.Title.Value.ShouldBe(title);
        result.Description.Value.ShouldBe(description);
        result.PlannedStart.Value.ShouldBe(plannedStart);
        result.PlannedFinish.Value.ShouldBe(plannedFinish);
    }
    
    [Fact]
    public void Create_GivenValidArgumentsWithoutPlannedStart_ShouldReturnProjectWithFilledFields()
    {
        //arrange
        var id = Guid.NewGuid();
        var title = "Test project";
        var description = "My description of test project";
        var plannedFinish = DateTime.Now.AddYears(1);
        
        //act
        var result = Project.Create(id, title, description, null, plannedFinish);
        
        //assert
        result.ShouldNotBeNull();
        result.Id.Value.ShouldBe(id);
        result.Title.Value.ShouldBe(title);
        result.Description.Value.ShouldBe(description);
        result.PlannedStart.ShouldBeNull();
        result.PlannedFinish.Value.ShouldBe(plannedFinish);
    }
    
    [Fact]
    public void Create_GivenValidArgumentsWithoutPlannedFinish_ShouldReturnProjectWithFilledFields()
    {
        //arrange
        var id = Guid.NewGuid();
        var title = "Test project";
        var description = "My description of test project";
        var plannedStart = DateTime.Now.AddYears(1);
        
        //act
        var result = Project.Create(id, title, description, plannedStart, null);
        
        //assert
        result.ShouldNotBeNull();
        result.Id.Value.ShouldBe(id);
        result.Title.Value.ShouldBe(title);
        result.Description.Value.ShouldBe(description);
        result.PlannedStart.Value.ShouldBe(plannedStart);
        result.PlannedFinish.ShouldBeNull();
    }
    
    [Fact]
    public void Create_GivenRequiredValidArguments_ShouldReturnProjectWithFilledFields()
    {
        //arrange
        var id = Guid.NewGuid();
        var title = "Test project";
        var description = "My description of test project";
        
        //act
        var result = Project.Create(id, title, description);
        
        //assert
        result.ShouldNotBeNull();
        result.Id.Value.ShouldBe(id);
        result.Title.Value.ShouldBe(title);
        result.Description.Value.ShouldBe(description);
        result.PlannedStart.ShouldBeNull();
        result.PlannedFinish.ShouldBeNull();
    }
    
    [Fact]
    public void Create_GivenEmptyTitle_ShouldThrowEmptyTitleException()
    {
        //act
        var exception = Record.Exception(() => Project.Create(Guid.NewGuid(), string.Empty, 
            "My description of test project"));
        
        //assert
        exception.ShouldBeOfType<EmptyTitleException>();
    }
    
    [Fact]
    public void Create_GivenPlannedStartLaterThanPlannedFinish_ShouldThrowInvalidDurationTimeException()
    {
        //act
        var exception = Record.Exception(() => Project.Create(Guid.NewGuid(), "Test project", 
            "My description of test project", DateTime.Now.AddYears(1), DateTime.Now.AddMonths(1)));
        
        //assert
        exception.ShouldBeOfType<InvalidDurationTimeException>();
    }
}