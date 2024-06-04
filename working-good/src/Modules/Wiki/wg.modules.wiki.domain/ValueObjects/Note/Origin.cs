using wg.modules.wiki.domain.Exceptions;

namespace wg.modules.wiki.domain.ValueObjects.Note;

public sealed record Origin
{
    private readonly List<string> _availableOrigins = ["Ticket", "Client"];
    public string Type { get; }
    public string Id { get; }

    public Origin(string type, string id)
    {
        if (!_availableOrigins.Contains(type))
        {
            throw new OriginTypeNoteAvailableException(type);
        }

        if (string.IsNullOrWhiteSpace(id))
        {
            throw new EmptyOriginIdValueException();
        }
        
        Type = type;
        Id = id;
    }
    

    public static string Ticket()
        => "Ticket";

    public static string Client()
        => "Client";
}