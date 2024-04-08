using wg.shared.abstractions.Exceptions;

namespace wg.modules.companies.application.Exceptions;

public sealed class SubstituteEmployeeIdNotFound(Guid substitutionEmployeeId)
    : WgException($"Employee with ID: {substitutionEmployeeId} for substitution not found");