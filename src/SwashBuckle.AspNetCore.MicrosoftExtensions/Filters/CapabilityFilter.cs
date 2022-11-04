using System.Collections.Generic;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Extensions;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;
using SwashBuckle.AspNetCore.MicrosoftExtensions.VendorExtensionEntities;

namespace SwashBuckle.AspNetCore.MicrosoftExtensions.Filters
{
    internal class CapabilityFilter : IDocumentFilter
    {
        private readonly FilePickerCapabilityModel m_filePickerCapability;

        public CapabilityFilter (FilePickerCapabilityModel capability)
        {
            m_filePickerCapability = capability;
        }

        public void Apply(OpenApiDocument swaggerDoc, DocumentFilterContext context)
        {
            AddFilePickerCapabilityExtension(swaggerDoc);
        }

        private void AddFilePickerCapabilityExtension(OpenApiDocument swaggerDoc)
        {
            swaggerDoc.AddExtension(
                Constants.XMsCapabilities,
                new OpenApiObject { {Constants.FilePicker, new OpenApiRawString(JsonConvert.SerializeObject(m_filePickerCapability))}}
            );
        }
    }
}