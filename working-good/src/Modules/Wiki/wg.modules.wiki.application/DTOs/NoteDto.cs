namespace wg.modules.wiki.application.DTOs;

public class NoteDto
{
    public Guid Id { get; set; }
    public string Title { get; set; }
    public string Content { get; set; }
    public string OriginId { get; set; }
    public string OriginType { get; set; }
}