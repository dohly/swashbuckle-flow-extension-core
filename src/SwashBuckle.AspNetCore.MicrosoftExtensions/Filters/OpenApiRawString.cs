using Microsoft.OpenApi;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Writers;
using Newtonsoft.Json;

namespace SwashBuckle.AspNetCore.MicrosoftExtensions.Filters
{
    /// <summary>
    /// Represents a raw value that should not be encoded
    /// </summary>
    internal class OpenApiRawString : IOpenApiAny, IOpenApiPrimitive
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="OpenApiRawString"/> class.
        /// </summary>
        /// <param name="value">Raw value</param>
        public OpenApiRawString(string value)
        {
            Value = value;
        }

        public OpenApiRawString(object o) : this(JsonConvert.SerializeObject(o)) { }

        /// <inheritdoc/>
        public AnyType AnyType => AnyType.Primitive;

        /// <inheritdoc/>
        public PrimitiveType PrimitiveType => PrimitiveType.String;

        /// <summary>
        /// Raw value to be output
        /// </summary>
        public string Value { get; }

        /// <inheritdoc/>
        public void Write(IOpenApiWriter writer, OpenApiSpecVersion specVersion)
        {
            writer.WriteRaw(Value);
        }
    }
}