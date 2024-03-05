using NSubstitute;
using wg.modules.owner.application.Auth;
using wg.modules.owner.application.CQRS.Users.Commands.SignIn;
using wg.modules.owner.domain.Repositories;
using wg.modules.owner.domain.ValueObjects.User;
using wg.modules.owner.tests.shared.Factories;
using Xunit;

namespace wg.modules.owner.application.tests.CQRS.Users.Commands;

public sealed class SignInCommandHandlerTests
{
    // private Task Act(SignInCommand command) => _handler.HandleAsync(command, default);
    //
    // [Fact]
    // public async Task Handle_GivenExistingEmailWithValidPassword_ShouldSaveTokenInStorage()
    // {
    //     //arrange
    //     var owner = OwnerFactory.Get();
    //     var user = UserFactory.GetUserInOwner(owner, Role.Manager());
    //     var command = new SignInCommand(user.Email, user.Password);
    //     _ownerRepository
    //         .GetAsync()
    //         .Returns(owner);
    //
    // }
    //
    // #region arrange
    // private readonly IOwnerRepository _ownerRepository;
    // private readonly IPasswordManager _passwordManager;
    // private readonly IAuthenticator _authenticator;
    // private readonly SignInCommandHandler _handler;
    //
    // public SignInCommandHandlerTests()
    // {
    //     _ownerRepository = Substitute.For<IOwnerRepository>();
    //     _passwordManager = Substitute.For<IPasswordManager>();
    //     _authenticator = Substitute.For<IAuthenticator>();
    //     _handler = new SignInCommandHandler(_ownerRepository, _passwordManager, _authenticator);
    // }
    // #endregion
}