using AeonRegistryAPI.Attributes;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Reflection;

namespace AeonRegistryAPI.Filters;

public class EnumStringFilter : ISchemaFilter
{
    public void Apply(OpenApiSchema schema, SchemaFilterContext context)
    {
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
