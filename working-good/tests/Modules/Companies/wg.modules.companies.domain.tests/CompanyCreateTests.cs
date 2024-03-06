using Shouldly;
using wg.modules.companies.domain.Entities;
using wg.modules.companies.domain.Exceptions;
using wg.modules.companies.domain.ValueObjects.Company;
using wg.shared.abstractions.Kernel.Exceptions;
using Xunit;

namespace wg.modules.companies.domain.tests;

public sealed class CompanyCreateTests
{
    [Fact]
    public void Create_GivenValidArguments_ShouldReturnCompanyWithFilledFields()
    {
        //arrange
        var id = Guid.NewGuid();
        var name = "MyCompanyName";
        var slaTime = TimeSpan.FromMinutes(10);
        var emailDomain = "test.pl";
        
        //act
        var result = Company.Create(id, name, slaTime, emailDomain);
        
        //assert
        result.Id.Value.ShouldBe(id);
        result.Name.Value.ShouldBe(name);
        result.SlaTime.Value.ShouldBe(slaTime);
        result.EmailDomain.Value.ShouldBe(emailDomain);
        result.Employees.Any().ShouldBeFalse();
        result.Projects.Any().ShouldBeFalse();
    }

    [Fact]
    public void Create_GivenEmptyName_ShouldThrowEmptyNameException()
    {
        //act
        var exception = Record.Exception(() => Company.Create(Guid.NewGuid(), string.Empty, 
            TimeSpan.FromMinutes(10), "test.pl"));
        
        //assert
        exception.ShouldBeOfType<EmptyNameException>();
    }
    
    [Fact]
    public void Create_GivenZeroSlaTime_ShouldThrowZeroSlaTimeException()
    {
        //act
        var exception = Record.Exception(() => Company.Create(Guid.NewGuid(), "MyCompanyName", 
            TimeSpan.Zero, "test.pl"));
        
        //assert
        exception.ShouldBeOfType<ZeroSlaTimeException>();
    }
    
    [Fact]
    public void Create_GivenEmptyEmailDomain_ShouldThrowEmptyNameException()
    {
        //act
        var exception = Record.Exception(() => Company.Create(Guid.NewGuid(), "MyCompanyName", 
            TimeSpan.FromMinutes(10), string.Empty));
        
        //assert
        exception.ShouldBeOfType<EmptyEmailDomainException>();
    }
}