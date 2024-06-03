using wg.shared.abstractions.CQRS.Commands;

namespace wg.modules.wiki.application.CQRS.Sections.Commands.ChangeParent;

public sealed record ChangeParentCommand(Guid SectionId, Guid? ParentSectionId) : ICommand;