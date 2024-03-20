using Shouldly;
using wg.modules.companies.infrastructure.Queries.Mappers;
using wg.sharedForTests.Factories.Companies;
using Xunit;

namespace wg.modules.companies.infrastructure.tests.Queries.Mappers;

public sealed class ExtensionsTests
{
    [Fact]
    public void AsSlaTimeDto_GivenCompany_ShouldReturnCompanySlaTimeDto()
    {
        //arrange
        var company = CompanyFactory.Get();
        
        //assert
        var result = company.AsSlaTimeDto();
        
        //assert
        result.ShouldNotBeNull();
        result.SlaTime.ShouldBe(company.SlaTime.Value);
    }
}