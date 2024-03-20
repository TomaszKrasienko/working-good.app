using System.Text.Json.Serialization;
using Newtonsoft.Json;
using wg.shared.infrastructure.Modules.Abstractions;

namespace wg.shared.infrastructure.Modules.Models;

internal sealed class ModuleTypesTranslator : IModuleTypesTranslator
{
    public object TranslateType(object value, Type type)
    {
        var sourceJson = FromObject(value);
        return JsonConvert.DeserializeObject(sourceJson, type);
    }

    public TResult TranslateType<TResult>(object value) where TResult : class
    {
        var sourceJson = FromObject(value);
        return JsonConvert.DeserializeObject<TResult>(sourceJson);
    }

    private string FromObject(object value)
        => JsonConvert.SerializeObject(value);
}