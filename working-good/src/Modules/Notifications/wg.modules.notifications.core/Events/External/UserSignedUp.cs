using wg.shared.abstractions.Events;

namespace wg.modules.notifications.core.Events.External;

public sealed record UserSignedUp(string Email, string FirstName, string LastName, string VerificationToken) : IEvent;