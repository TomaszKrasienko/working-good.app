using wg.shared.abstractions.CQRS.Commands;

namespace wg.modules.wiki.application.CQRS.Sections.Commands;

public record AddSectionCommand(Guid Id, string Name, Guid? ParentId) : ICommand;