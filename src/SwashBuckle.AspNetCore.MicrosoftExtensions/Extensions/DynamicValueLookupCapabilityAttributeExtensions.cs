using System.Collections.Generic;
using Microsoft.OpenApi.Interfaces;
using SwashBuckle.AspNetCore.MicrosoftExtensions.Attributes;
using SwashBuckle.AspNetCore.MicrosoftExtensions.Filters;
using SwashBuckle.AspNetCore.MicrosoftExtensions.Helpers;
using SwashBuckle.AspNetCore.MicrosoftExtensions.VendorExtensionEntities;

namespace SwashBuckle.AspNetCore.MicrosoftExtensions.Extensions
{
    internal static class DynamicValueLookupCapabilityAttributeExtensions
    {
        internal static IEnumerable<KeyValuePair<string, IOpenApiExtension>> GetSwaggerExtensions(this DynamicValueLookupCapabilityAttribute attribute)
        {
            if (attribute is null)
                yield break;

            yield return new KeyValuePair<string, IOpenApiExtension>
            (
                Constants.XMsDynamicValueLookup,
                new OpenApiRawString(
                new DynamicValuesCapabilityModel
                (
                    attribute.Capability,
                    attribute.ValuePath,
                    attribute.ValueTitle,
                    ParameterParser.Parse(attribute.Parameters)
                ))
            );
        }
    }
}