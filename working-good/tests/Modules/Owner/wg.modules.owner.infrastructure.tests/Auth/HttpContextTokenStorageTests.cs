using Microsoft.AspNetCore.Http;
using NSubstitute;
using Shouldly;
using wg.modules.owner.infrastructure.Auth;
using wg.modules.owner.tests.shared.Factories;
using Xunit;

namespace wg.modules.owner.infrastructure.tests.Auth;

public sealed class HttpContextTokenStorageTests
{
    [Fact]
    public void Set_GivenToken_ShouldSetTokenToHttpContext()
    {
        //arrange
        var jwtToken = JwtDtoFactory.Get();
        _httpContextAccessor.HttpContext = new DefaultHttpContext();
        
        //act
        _storage.Set(jwtToken);
        
        //assert
        _httpContextAccessor.HttpContext.Items.TryGetValue("user_jwt_token", out var token);
        token.ShouldBe(jwtToken);
    }

    [Fact]
    public void Get_ForExistingJwtToken_ShouldReturnJwtToken()
    {
        //arrange
        var jwtToken = JwtDtoFactory.Get();
        _httpContextAccessor.HttpContext!.Items.TryAdd("user_jwt_token", jwtToken);
        
        //act
        var result = _storage.Get();
        
        //assert
        result.ShouldBe(jwtToken);
    }
    
    [Fact]
    public void Get_ForNotExistingJwtToken_ShouldReturnNull()
    {
        //act
        var result = _storage.Get();
        
        //assert
        result.ShouldBeNull();
    }
    
    #region arrange
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly HttpContextTokenStorage _storage;

    public HttpContextTokenStorageTests()
    {
        _httpContextAccessor = Substitute.For<IHttpContextAccessor>();
        _httpContextAccessor.HttpContext = new DefaultHttpContext();
        _storage = new HttpContextTokenStorage(_httpContextAccessor);
    }
    #endregion
}