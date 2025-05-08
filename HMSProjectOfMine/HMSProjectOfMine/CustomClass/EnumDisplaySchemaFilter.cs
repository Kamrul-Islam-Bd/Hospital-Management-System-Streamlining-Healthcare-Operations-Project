//using Microsoft.OpenApi.Models;
//using Swashbuckle.AspNetCore.SwaggerGen;
//using System.ComponentModel.DataAnnotations;
//using System.Reflection;

//namespace HMSProjectOfMine.CustomClass
//{
//    //Enum ke swagger a string akare dekhanor jonno, 0,1,2 er bodole
//    public class EnumDisplaySchemaFilter : ISchemaFilter
//    {
//        public void Apply(OpenApiSchema schema, SchemaFilterContext context)
//        {
//            if (!context.Type.IsEnum) return;

//            var enumNames = Enum.GetNames(context.Type);
//            var enumType = context.Type;

//            schema.Enum.Clear(); // clear the default enum values

//            foreach (var name in enumNames)
//            {
//                var field = enumType.GetField(name);
//                var displayAttr = field?.GetCustomAttribute<DisplayAttribute>();
//                var displayName = displayAttr?.Name ?? name;

//                schema.Enum.Add(new Microsoft.OpenApi.Any.OpenApiString(displayName));
//            }

//            schema.Type = "string";
//            schema.Format = null;
//        }
//    }
//}
