using wg.shared.abstractions.Exceptions;

namespace wg.modules.owner.domain.Exceptions;

public sealed class VerificationTokenNotFoundException(string token)
    :  WgException($"Verification token with value: {token} not found");