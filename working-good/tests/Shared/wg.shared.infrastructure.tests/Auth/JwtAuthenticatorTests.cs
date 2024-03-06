using Microsoft.Extensions.Options;
using Shouldly;
using wg.shared.abstractions.Auth;
using wg.shared.abstractions.Auth.DTOs;
using wg.shared.abstractions.Time;
using wg.shared.infrastructure.Auth;
using wg.shared.infrastructure.Auth.Configuration.Models;
using wg.sharedForTests.Factories.Jwt;
using wg.sharedForTests.Mocks;
using Xunit;

namespace wg.shared.infrastructure.tests.Auth;

public class JwtAuthenticatorTests
{
    [Fact]
    public void CreateToken_GivenUserIdAndRole_ShouldReturnJwtToken()
    {
        //arrange
        var userId = Guid.NewGuid();
        var role = "Role";
        
        //act
        var result = _authenticator.CreateToken(userId.ToString(), role);
        
        //assert
        result.ShouldNotBeNull();
        result.ShouldBeOfType<JwtDto>();
        result.Token.ShouldNotBeNullOrWhiteSpace();
    }
    
    #region arrange
    private readonly JwtOptions _jwtOptions;
    private readonly IClock _clock;
    private readonly IAuthenticator _authenticator;

    public JwtAuthenticatorTests()
    {
        _jwtOptions = JwtOptionsFactory.Get();
        var options = Options.Create(_jwtOptions);
        _clock = TestsClock.Create();
        _authenticator = new JwtAuthenticator(_clock, options);
    }
    #endregion
}