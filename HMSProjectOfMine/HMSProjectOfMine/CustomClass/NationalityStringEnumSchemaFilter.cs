using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace HMSProjectOfMine.CustomClass
{
    public class NationalityStringEnumSchemaFilter : ISchemaFilter
    {
        public void Apply(OpenApiSchema schema, SchemaFilterContext context)
        {
            if (context.Type == typeof(string) && context.MemberInfo?.Name == "Nationality")
            {
                schema.Enum = new List<IOpenApiAny>
            {
                new OpenApiString("Bangladeshi"),
                new OpenApiString("Indian"),
                new OpenApiString("American"),
                new OpenApiString("Canadian"),
                new OpenApiString("British")
                // Add more as needed
            };
            }
        }
    }
}
