namespace wg.shared.infrastructure.Modules.Abstractions;

public interface IModuleTypesTranslator
{
    object TranslateType(object value, Type type);
    TResult TranslateType<TResult>(object value) where TResult : class;
}