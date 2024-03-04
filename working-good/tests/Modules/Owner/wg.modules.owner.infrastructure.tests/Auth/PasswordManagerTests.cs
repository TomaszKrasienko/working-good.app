using Bogus;
using Microsoft.AspNetCore.Identity;
using NSubstitute;
using Shouldly;
using wg.modules.owner.application.Auth;
using wg.modules.owner.domain.Entities;
using wg.modules.owner.infrastructure.Auth;
using Xunit;

namespace wg.modules.owner.infrastructure.tests.Auth;

public sealed class PasswordManagerTests
{
    [Fact]
    public void Secure_GivenPassword_ShouldReturnHashedPassword()
    {
        //arrange
        var passwordFaker = new Faker<string>().CustomInstantiator(f => f.Lorem.Word());
        var passwords = passwordFaker.Generate(2);
        _passwordHasher.HashPassword(default!, passwords[0]).Returns(passwords[1]);
        
        //act
        var result = _passwordManager.Secure(passwords[0]);
        
        //assert
        result.ShouldBe(passwords[1]);
    }
    
    #region arrange

    private readonly IPasswordHasher<User> _passwordHasher;
    private readonly IPasswordManager _passwordManager;

    public PasswordManagerTests()
    {
        _passwordHasher = Substitute.For<IPasswordHasher<User>>();
        _passwordManager = new PasswordManager(_passwordHasher);
    }
    #endregion
}