using wg.modules.owner.domain.ValueObjects.User;
using wg.shared.abstractions.Kernel.Types;
using wg.shared.abstractions.Kernel.ValueObjects;

namespace wg.modules.owner.domain.Entities;

public sealed class User
{
    public EntityId Id { get; }
    public Email Email { get; }
    public FullName FullName { get; }
    public Password Password { get; }
    public Role Role { get; }
    public VerificationToken VerificationToken { get; }
    public ResetPasswordToken ResetPasswordToken { get; }
    public State State { get; private set; }

    private User(EntityId id, Email email, FullName fullName, Password password, Role role, 
        VerificationToken verificationToken, ResetPasswordToken resetPasswordToken, State state)
    {
        Id = id;
        Email = email;
        FullName = fullName;
        Password = password;
        Role = role;
        VerificationToken = verificationToken;
        ResetPasswordToken = resetPasswordToken;
    }

    private User(EntityId id, Email email, FullName fullName, Password password, Role role,
        VerificationToken verificationToken)
    {
        Id = id;
        Email = email;
        FullName = fullName;
        Password = password;
        Role = role;
        VerificationToken = verificationToken;
        State = State.Registered();
    }

    internal static User Create(Guid id, string email, string firstName, string lastName, string password,
        string role)
        => new User(id, email, new FullName(firstName, lastName), password, role, VerificationToken.Create());

    public void Verify(DateTime verificationDateTime)
    {
        VerificationToken.Verify(verificationDateTime);
        State = State.Activate();
    }
}