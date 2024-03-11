namespace wg.shared.infrastructure.Modules.Abstractions;

public interface IModuleTypesTranslator
{
    object TranslateType(object value, Type type);
}