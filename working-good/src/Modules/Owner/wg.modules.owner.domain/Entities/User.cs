using wg.modules.owner.domain.ValueObjects.User;
using wg.shared.abstractions.Kernel.Types;

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

    private User(EntityId id, Email email, FullName fullName, Password password, Role role, 
        VerificationToken verificationToken, ResetPasswordToken resetPasswordToken)
    {
        Id = id;
        Email = email;
        FullName = fullName;
        Password = password;
        Role = role;
        VerificationToken = verificationToken;
        ResetPasswordToken = resetPasswordToken;
    }

    private User(EntityId id, Email email, FullName fullName, Password password, Role role, VerificationToken verificationToken)
    {
        Id = id;
        Email = email;
        FullName = fullName;
        Password = password;
        Role = role;
        VerificationToken = verificationToken;
    }
}