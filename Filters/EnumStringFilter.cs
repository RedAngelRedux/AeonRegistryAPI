using AeonRegistryAPI.Attributes;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Reflection;

namespace AeonRegistryAPI.Filters;

public class EnumStringFilter : ISchemaFilter
{
    public void Apply(OpenApiSchema schema, SchemaFilterContext context)
    {
        //// Coder Foundry's Version
        //if(context.Type == typeof(string) && context.MemberInfo?.Name == "Type")
        //    schema.Description = "Allowed Values: " + string.Join(", ",Enum.GetNames(typeof(ArtifactType)));

        var propertyInfo = context.MemberInfo as PropertyInfo;
        if (propertyInfo == null)
            return;

        var enumAttr = propertyInfo.GetCustomAttribute<EnumStringAttribute>();
        if (enumAttr == null)
            return;

        var enumNames = Enum.GetNames(enumAttr.EnumType);
        schema.Description = $"Allowed Values: {string.Join(", ", enumNames)}";

    }
}
