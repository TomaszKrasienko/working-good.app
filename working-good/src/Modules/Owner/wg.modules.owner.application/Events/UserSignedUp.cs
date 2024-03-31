using wg.shared.abstractions.Events;

namespace wg.modules.owner.application.Events;

public sealed record UserSignedUp(string Email, string FirstName, string LastName, string VerificationToken) : IEvent;