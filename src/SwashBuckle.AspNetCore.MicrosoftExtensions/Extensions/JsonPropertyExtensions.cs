﻿using System.Collections.Generic;
using System.Linq;
using Microsoft.OpenApi.Interfaces;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json.Serialization;
using Swashbuckle.AspNetCore.Swagger;
using SwashBuckle.AspNetCore.MicrosoftExtensions.Attributes;

namespace SwashBuckle.AspNetCore.MicrosoftExtensions.Extensions
{
    internal static class JsonPropertyExtensions
    {
        internal static void ExtendProperties(this IDictionary<string, OpenApiSchema> schemaProperties, JsonPropertyCollection jsonProperties)
        {
            if (schemaProperties is null)
                return;
            
            foreach (var schemaProperty in schemaProperties)
            {
                var jsonProperty = jsonProperties.FirstOrDefault(x => x.PropertyName == schemaProperty.Key);
                schemaProperty.Value.ExtendProperty(jsonProperty);
            }
        }

        private static void ExtendProperty (this OpenApiSchema schema, JsonProperty jsonProperty)
        {
            schema.Extensions.AddRange(GetMetadataExtensions(jsonProperty.AttributeProvider));
            schema.Extensions.AddRange(GetValueLookupProperties(jsonProperty.AttributeProvider));
            schema.Extensions.AddRange(GetSchemaLookupProperties(jsonProperty.AttributeProvider));
            schema.Extensions.AddRange(GetValueLookupCapabilityProperties(jsonProperty.AttributeProvider));
        }
        
        private static IEnumerable<KeyValuePair<string, IOpenApiExtension>> GetMetadataExtensions(IAttributeProvider attributeProvider)
        {
            var attribute = attributeProvider.GetAttributes(typeof(MetadataAttribute), false).SingleOrDefault() as MetadataAttribute;
            return attribute.GetSwaggerExtensions();
        }

        private static IEnumerable<KeyValuePair<string, IOpenApiExtension>> GetValueLookupProperties(IAttributeProvider attributeProvider)
        {
            var attribute = attributeProvider.GetAttributes(typeof(DynamicValueLookupAttribute), true).SingleOrDefault() as DynamicValueLookupAttribute;
            return attribute.GetSwaggerExtensions();
        }

        private static IEnumerable<KeyValuePair<string, IOpenApiExtension>> GetValueLookupCapabilityProperties (IAttributeProvider attributeProvider)
        {
            var attribute = attributeProvider.GetAttributes(typeof(DynamicValueLookupCapabilityAttribute), true).SingleOrDefault() as DynamicValueLookupCapabilityAttribute;
            return attribute.GetSwaggerExtensions();
        }

        private static IEnumerable<KeyValuePair<string, IOpenApiExtension>> GetSchemaLookupProperties
            (IAttributeProvider attributeProvider)
        {
            var attribute = attributeProvider.GetAttributes(typeof(DynamicSchemaLookupAttribute), true).SingleOrDefault() as DynamicSchemaLookupAttribute;
            return attribute.GetSwaggerExtensions();
        }
    }
}