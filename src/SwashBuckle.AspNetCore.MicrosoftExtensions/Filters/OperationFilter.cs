using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.OpenApi.Interfaces;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;
using SwashBuckle.AspNetCore.MicrosoftExtensions.Attributes;
using SwashBuckle.AspNetCore.MicrosoftExtensions.Extensions;

namespace SwashBuckle.AspNetCore.MicrosoftExtensions.Filters
{
    internal class OperationFilter : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            if(operation is null || context is null)
                return;

            operation.Extensions.AddRange(GetOperationExtensions(context.ApiDescription));

            ApplyParametersMetadata(operation.Parameters, context.ApiDescription.ActionDescriptor.Parameters);
        }

        private static IEnumerable<KeyValuePair<string, IOpenApiExtension>> GetOperationExtensions(ApiDescription apiDescription)
        {
            if (apiDescription.TryGetMethodInfo(out var mi))
            {
                var metadataAttribute = mi.CustomAttributes.OfType<MetadataAttribute>().FirstOrDefault();
                var dynamicSchemaAttribute = mi.CustomAttributes.OfType<DynamicSchemaLookupAttribute>().FirstOrDefault();
                var extensions = metadataAttribute.GetSwaggerOperationExtensions();
                return extensions.Concat(dynamicSchemaAttribute.GetSwaggerExtensions());
            }
            return Enumerable.Empty<KeyValuePair<string, IOpenApiExtension>>();
        }

        private static void ApplyParametersMetadata
        (
            IEnumerable<OpenApiParameter> parameters,
            IList<ParameterDescriptor> parameterDescriptions
        )
        {
            if(parameters is null) 
                return;
            
            foreach (var operationParameter in parameters)
            {
                var parameterDescription = parameterDescriptions.FirstOrDefault(x => x.Name == operationParameter.Name);
                switch (parameterDescription)
                {
                    case ControllerParameterDescriptor controllerParameterDescriptor:
                        operationParameter.Extensions.AddRange(GetParameterExtensions(controllerParameterDescriptor.ParameterInfo));
                        break;
                    case ControllerBoundPropertyDescriptor controllerBoundPropertyDescriptor:
                        operationParameter.Extensions.AddRange(GetParameterExtensions(controllerBoundPropertyDescriptor.PropertyInfo));
                        break;
                    default:
                        continue;
                }
            }
        }

        private static IEnumerable<KeyValuePair<string, IOpenApiExtension>> GetParameterExtensions(ICustomAttributeProvider attributeProvider)
        {
            return GetValueLookupProperties(attributeProvider)
                .Concat(GetValueLookupCapabilityProperties(attributeProvider))
                .Concat(GetMetadataProperties(attributeProvider))
                .Concat(GetSchemaLookupProperties(attributeProvider));
        }

        private static IEnumerable<KeyValuePair<string, IOpenApiExtension>> GetValueLookupProperties(ICustomAttributeProvider attributeProvider)
        {
            var attribute = attributeProvider.GetCustomAttributes(typeof(DynamicValueLookupAttribute), true).SingleOrDefault() as DynamicValueLookupAttribute;
            return attribute.GetSwaggerExtensions();
        }

        private static IEnumerable<KeyValuePair<string, IOpenApiExtension>> GetValueLookupCapabilityProperties (ICustomAttributeProvider attributeProvider)
        {
            var attribute = attributeProvider.GetCustomAttributes(typeof(DynamicValueLookupCapabilityAttribute), true).SingleOrDefault() as DynamicValueLookupCapabilityAttribute;
            return attribute.GetSwaggerExtensions();
        }

        private static IEnumerable<KeyValuePair<string, IOpenApiExtension>> GetMetadataProperties(ICustomAttributeProvider attributeProvider)
        {
            var attribute = attributeProvider.GetCustomAttributes(typeof(MetadataAttribute), true).SingleOrDefault() as MetadataAttribute;
            return attribute.GetSwaggerExtensions();
        }

        private static IEnumerable<KeyValuePair<string, IOpenApiExtension>> GetSchemaLookupProperties(ICustomAttributeProvider attributeProvider)
        {
            var attribute = attributeProvider.GetCustomAttributes(typeof(DynamicSchemaLookupAttribute), true).SingleOrDefault() as DynamicSchemaLookupAttribute;
            return attribute.GetSwaggerExtensions();
        }
    }
}