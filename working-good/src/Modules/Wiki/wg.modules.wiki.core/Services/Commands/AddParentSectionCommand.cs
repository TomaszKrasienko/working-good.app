namespace wg.modules.wiki.core.Services.Commands;

public sealed record AddParentSectionCommand(Guid SectionId, Guid ParentSectionId);