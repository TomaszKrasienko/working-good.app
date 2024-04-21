using System.Text.Json;
using Microsoft.Extensions.Configuration;

namespace wg.shared.infrastructure.Vault;


internal sealed class JsonParser
{
private readonly Dictionary<string, string> _data = new(StringComparer.OrdinalIgnoreCase);

private readonly Stack<string> _stack = new();

public IDictionary<string, string> Parse(string json)
{
    var jsonDocumentOptions = new JsonDocumentOptions
    {
        CommentHandling = JsonCommentHandling.Skip,
        AllowTrailingCommas = true,
    };

    using (JsonDocument doc = JsonDocument.Parse(json, jsonDocumentOptions))
    {
        if (doc.RootElement.ValueKind != JsonValueKind.Object)
        {
            throw new FormatException($"Invalid top level JSON element: {doc.RootElement.ValueKind}");
        }

        VisitElement(doc.RootElement);
    }

        return _data;
    }

    private void VisitElement(JsonElement element)
    {
    var isEmpty = true;

    foreach (JsonProperty property in element.EnumerateObject())
    {
    isEmpty = false;
    EnterContext(property.Name);
    VisitValue(property.Value);
    ExitContext();
    }

    if (isEmpty && _stack.Count > 0)
    {
    _data[_stack.Peek()] = null;
    }
    }

    private void VisitValue(JsonElement value)
    {
    switch (value.ValueKind)
    {
    case JsonValueKind.Object:
        VisitElement(value);

        break;

    case JsonValueKind.Array:
        int index = 0;

        foreach (JsonElement arrayElement in value.EnumerateArray())
        {
            EnterContext(index.ToString());
            VisitValue(arrayElement);
            ExitContext();
            index++;
        }

        break;

    case JsonValueKind.Number:
    case JsonValueKind.String:
    case JsonValueKind.True:
    case JsonValueKind.False:
    case JsonValueKind.Null:
        var key = _stack.Peek();

        if (_data.ContainsKey(key))
        {
            throw new FormatException($"Duplicated key: {key}");
        }

        _data[key] = value.ToString();

        break;

    default:
        throw new FormatException($"Unsupported JSON token: {value.ValueKind}");
    }
    }

    private void EnterContext(string context) =>
    _stack.Push(_stack.Count > 0
    ? _stack.Peek() + ConfigurationPath.KeyDelimiter + context
    : context);

    private void ExitContext() => _stack.Pop();
}
