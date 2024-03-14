namespace wg.modules.owner.domain.Exceptions;

public class GroupAlreadyExistsException(string title) 
    : Exception($"Group with title: {title} already registered");