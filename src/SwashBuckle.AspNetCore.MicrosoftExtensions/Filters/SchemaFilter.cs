using System.Collections.Generic;
using System.Reflection;
using Microsoft.OpenApi.Interfaces;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json.Serialization;
using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;
using SwashBuckle.AspNetCore.MicrosoftExtensions.Attributes;
using SwashBuckle.AspNetCore.MicrosoftExtensions.Extensions;

namespace SwashBuckle.AspNetCore.MicrosoftExtensions.Filters
{
    internal class SchemaFilter : ISchemaFilter
    {
        public void Apply(OpenApiSchema model, SchemaFilterContext context)
        {
            if(model is null || context is null)
            {
                return;
            }

            model.Extensions.AddRange(GetClassExtensions(context));

            //if (context.JsonContract is JsonObjectContract objectContract)
            //{
            //    model.Properties.ExtendProperties(objectContract.Properties);
            //}
        }

        private IEnumerable<KeyValuePair<string, IOpenApiExtension>> GetClassExtensions(SchemaFilterContext context)
        {
            var attribute = context.Type.GetTypeInfo().GetCustomAttribute<DynamicSchemaLookupAttribute>();
            return attribute.GetSwaggerExtensions();
        }
    }
}