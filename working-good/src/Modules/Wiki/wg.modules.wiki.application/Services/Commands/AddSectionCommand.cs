namespace wg.modules.wiki.core.Services.Commands;

public sealed record AddSectionCommand(Guid Id, string Name, Guid? ParentId);