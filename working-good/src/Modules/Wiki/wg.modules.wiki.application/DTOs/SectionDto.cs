namespace wg.modules.wiki.application.DTOs;

public class SectionDto
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public IReadOnlyList<NoteDto> Notes { get; set; }
}