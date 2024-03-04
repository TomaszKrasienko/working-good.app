using System.Reflection;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;

namespace wg.shared.infrastructure.Providers;

internal sealed class InternalControllerFeatureProvider : ControllerFeatureProvider
{
    protected override bool IsController(TypeInfo typeInfo)
    {
        return typeInfo.IsClass && 
               !typeInfo.IsAbstract && 
               !typeInfo.ContainsGenericParameters && 
               !typeInfo.IsDefined(typeof (NonControllerAttribute)) && 
               (typeInfo.Name.EndsWith("Controller", StringComparison.OrdinalIgnoreCase) || typeInfo.IsDefined(typeof (ControllerAttribute)));
    }
}