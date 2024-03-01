using wg.shared.abstractions.Exceptions;

namespace wg.modules.owner.domain.Exceptions;

public sealed class EmptyFullNameFieldException(string fieldName)
    : WgException($"Value: {fieldName} can not be empty")
{
    
}