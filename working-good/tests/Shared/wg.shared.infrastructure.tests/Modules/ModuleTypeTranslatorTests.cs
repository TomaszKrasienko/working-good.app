using Microsoft.EntityFrameworkCore.Metadata;
using Shouldly;
using wg.shared.infrastructure.Modules.Abstractions;
using wg.shared.infrastructure.Modules.Models;
using Xunit;

namespace wg.shared.infrastructure.tests.Modules;

public sealed class ModuleTypeTranslatorTests
{
    [Fact]
    public void Tmp_GivenTypeAndObject_ShouldTranslateType()
    {
        //arrange
        TypeSource typeSource = new TypeSource()
        {
            Value = "temporary_value"
        };
        
        //act
        var result = _moduleTypesTranslator.TranslateType(typeSource, typeof(TypeDestination));
        
        //assert
        result.ShouldBeOfType<TypeDestination>();
        (result as TypeDestination)!.Value.ShouldBe(typeSource.Value);
    }
    
    #region arrange
    private readonly IModuleTypesTranslator _moduleTypesTranslator;
    public ModuleTypeTranslatorTests()
    {
        _moduleTypesTranslator = new ModuleTypesTranslator();
    }
    #endregion
}

public class TypeSource
{
    public string Value { get; set; }
}

public class TypeDestination
{
    public string Value { get; set; }
}