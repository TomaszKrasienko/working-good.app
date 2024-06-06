using wg.modules.wiki.application.DTOs;
using wg.shared.abstractions.CQRS.Queries;

namespace wg.modules.wiki.application.CQRS.Sections.Queries;

public sealed record GetSectionByIdQuery(Guid SectionId) : IQuery<SectionDto>;