namespace wg.modules.wiki.core.Services.Commands;

public sealed record ChangeParentSectionCommand(Guid SectionId, Guid? ParentSectionId);