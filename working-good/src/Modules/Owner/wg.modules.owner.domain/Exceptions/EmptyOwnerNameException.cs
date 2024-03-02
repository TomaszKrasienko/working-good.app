using wg.shared.abstractions.Exceptions;

namespace wg.modules.owner.domain.Exceptions;

public sealed class EmptyOwnerNameException()
: WgException("Owner name can not be empty");