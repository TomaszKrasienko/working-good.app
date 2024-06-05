using NSubstitute;
using Shouldly;
using wg.modules.wiki.application.Clients.Companies;
using wg.modules.wiki.application.Clients.Companies.DTOs;
using wg.modules.wiki.application.Exceptions;
using wg.modules.wiki.application.Strategies.Origins;
using Xunit;

namespace wg.modules.wiki.application.tests.Strategies.Origins;

public sealed class ClientCheckingStrategyTests
{
    [Fact]
    public async Task IsExists_GivenExistingActiveClient_ShouldReturnTrue()
    {
        //arrange
        var originId = Guid.NewGuid();
        _companiesApiClient
            .IsActiveCompanyExistsAsync(new CompanyIdDto(originId))
            .Returns(new IsActiveCompanyExistsDto(true));
        
        //act
        var result = await _strategy.IsExists(originId.ToString());

        //assert
        result.ShouldBeTrue();
    }
    
    [Fact]
    public async Task IsExists_GivenNotExistingActiveClient_ShouldReturnFalse()
    {
        //arrange
        var originId = Guid.NewGuid();
        _companiesApiClient
            .IsActiveCompanyExistsAsync(new CompanyIdDto(originId))
            .Returns(new IsActiveCompanyExistsDto(false));
        
        //act
        var result = await _strategy.IsExists(originId.ToString());

        //assert
        result.ShouldBeFalse();
    }
    
    [Fact]
    public async Task IsExists_GivenInvalidGuid_ShouldThrowOriginIdIsInvalidException()
    {
        //arrange
        var originId = "invalid_guid";
        
        //act
        var exception = await Record.ExceptionAsync(async () => await _strategy.IsExists(originId));
        
        //assert
        exception.ShouldBeOfType<OriginIdIsInvalidException>();
    }
    
    #region arrange
    private readonly ICompaniesApiClient _companiesApiClient;
    private readonly IOriginCheckingStrategy _strategy;

    public ClientCheckingStrategyTests()
    {
        _companiesApiClient = Substitute.For<ICompaniesApiClient>();
        _strategy = new ClientCheckingStrategy(_companiesApiClient);
    }
    #endregion
}