using System.Text.Json.Serialization;
using Newtonsoft.Json;
using wg.shared.infrastructure.Modules.Abstractions;

namespace wg.shared.infrastructure.Modules.Models;

internal sealed class ModuleTypesTranslator : IModuleTypesTranslator
{
    public object TranslateType(object value, Type type)
    {
        var sourceJson = FromObject(value);
        return ToObject(type, sourceJson);
    }

    private string FromObject(object value)
        => JsonConvert.SerializeObject(value);

    private object ToObject(Type type, string json)
        => JsonConvert.DeserializeObject(json, type);
}