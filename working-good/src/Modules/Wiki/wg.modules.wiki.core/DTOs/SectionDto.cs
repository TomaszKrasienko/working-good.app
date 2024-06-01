namespace wg.modules.wiki.core.DTOs;

public class SectionDto
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public List<SectionDto> Children { get; set; }
}