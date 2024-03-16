using wg.modules.owner.domain.ValueObjects.User;
using wg.shared.abstractions.Kernel.Types;
using wg.shared.abstractions.Kernel.ValueObjects;

namespace wg.modules.owner.domain.Entities;

public sealed class User
{
    public EntityId Id { get; }
    public Email Email { get; private set; }
    public FullName FullName { get; private set; }
    public Password Password { get; private set; }
    public Role Role { get; private set; }
    public VerificationToken VerificationToken { get; }
    public ResetPasswordToken ResetPasswordToken { get; }
    public State State { get; private set; }
    private HashSet<Group> _groups = new HashSet<Group>();
    public IEnumerable<Group> Groups => _groups;

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

    private User(EntityId id)
    {
        Id = id;
        VerificationToken = VerificationToken.Create();
        State = State.Registered();
    }

    internal static User Create(Guid id, string email, string firstName, string lastName, string password,
        string role)
    {
        var user = new User(id);
        user.ChangeEmail(email);
        user.ChangeFullName(firstName, lastName);
        user.ChangePassword(password);
        user.ChangeRole(role);
        return user;
    }

    private void ChangeEmail(string email)
        => Email = email;

    private void ChangeFullName(string firstName, string lastName)
        => FullName = new FullName(firstName, lastName);

    private void ChangePassword(string password)
        => Password = password;

    private void ChangeRole(string role)
        => Role = role;
    
    public void Verify(DateTime verificationDateTime)
    {
        VerificationToken.Verify(verificationDateTime);
        State = State.Activate();
    }
}