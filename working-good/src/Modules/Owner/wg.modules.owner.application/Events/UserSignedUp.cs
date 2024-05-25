using wg.shared.abstractions.Events;

namespace wg.modules.owner.application.Events;

public sealed record UserSignedUp(Guid Id, string Email, string FirstName, string LastName, string VerificationToken) : IEvent;