using wg.shared.abstractions.Kernel.Types;

namespace wg.modules.owner.domain.Entities;

public sealed class User
{
    public EntityId Id { get; }
    public string Email { get; set; }
    public string FullName { get; set; }
    public string Password { get; set; }
    public string Role { get; set; }
    public string VerificationToken { get; set; }
    public string ResetToken { get; set; }
}