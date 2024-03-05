using NSubstitute;
using wg.modules.owner.application.Auth;
using wg.modules.owner.application.CQRS.Users.Commands.SignIn;
using wg.modules.owner.application.DTOs;
using wg.modules.owner.domain.Repositories;
using wg.modules.owner.domain.ValueObjects.User;
using wg.modules.owner.tests.shared.Factories;
using wg.shared.abstractions.Time;
using wg.shared.tests.shared.Mocks;
using Xunit;

namespace wg.modules.owner.application.tests.CQRS.Users.Commands;

public sealed class SignInCommandHandlerTests
{
    private Task Act(SignInCommand command) => _handler.HandleAsync(command, default);
    
    [Fact]
    public async Task Handle_GivenExistingEmailWithValidPassword_ShouldSaveTokenInStorage()
    {
        //arrange
        var owner = OwnerFactory.Get();
        var user = UserFactory.GetUserInOwner(owner, Role.Manager());
        owner.VerifyUser(user.VerificationToken.Token, _clock.Now());
        var command = new SignInCommand(user.Email, user.Password);
        var jwtDto = JwtDtoFactory.Get();
        _ownerRepository
            .GetAsync()
            .Returns(owner);
        _passwordManager
            .VerifyPassword(user.Password, command.Password)
            .Returns(true);
        _authenticator
            .CreateToken(user.Id.ToString(), user.Role)
            .Returns(jwtDto);
        
        //act
        await Act(command);
        
        //assert
        _tokenStorage
            .Received(1)
            .Set(jwtDto);
    }
    
    #region arrange
    private readonly IOwnerRepository _ownerRepository;
    private readonly IPasswordManager _passwordManager;
    private readonly IAuthenticator _authenticator;
    private readonly IClock _clock;
    private readonly ITokenStorage _tokenStorage;
    private readonly SignInCommandHandler _handler;
    
    public SignInCommandHandlerTests()
    {
        _ownerRepository = Substitute.For<IOwnerRepository>();
        _passwordManager = Substitute.For<IPasswordManager>();
        _authenticator = Substitute.For<IAuthenticator>();
        _clock = TestsClock.Create();
        _tokenStorage = Substitute.For<ITokenStorage>();
        _handler = new SignInCommandHandler(_ownerRepository, _passwordManager, _authenticator, 
            _clock, _tokenStorage);
    }
    #endregion
}