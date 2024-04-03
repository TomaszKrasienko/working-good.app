using NSubstitute;
using Shouldly;
using wg.shared.abstractions.Pagination;
using wg.shared.infrastructure.Pagination.Mappers;
using wg.tests.shared.Factories;
using wg.tests.shared.Models;
using Xunit;

namespace wg.shared.infrastructure.tests.Pagination;

public sealed class ExtensionsTests
{
    [Fact]
    public void AsMetaData_GivenPagedList_ShouldReturnMetaDataDto()
    {
        //arrange
        var dtoList = TestDtoFactory.Get(10);
        var pagedList = new PagedList<TestDto>(dtoList, 2, 3, 4);
        
        //act
        var result = pagedList.AsMetaData();
        
        //assert
        result.CurrentPage.ShouldBe(pagedList.CurrentPage);
        result.TotalPages.ShouldBe(pagedList.TotalPages);
        result.PageSize.ShouldBe(pagedList.PageSize);
        result.TotalCount.ShouldBe(pagedList.TotalCount);
        result.HasPrevious.ShouldBe(pagedList.HasPrevious);
        result.HasNext.ShouldBe(pagedList.HasNext);
    } 
}