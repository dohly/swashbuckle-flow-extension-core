using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using SwashBuckle.AspNetCore.MicrosoftExtensions.Extensions;
using SwashBuckle.AspNetCore.MicrosoftExtensions.VendorExtensionEntities;
using System.Collections.Generic;

namespace TestApi
{
    public class Program
    {
        private static FilePickerCapabilityModel FilePicker =>
          new FilePickerCapabilityModel
          (
              new FilePickerOperationModel("InitialOperation", null),
              new FilePickerOperationModel("BrowsingOperation", new Dictionary<string, string> { { "Id", "Id" } }),
              "Name",
              "IsFolder",
              "MediaType"
          );

        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1" });
                c.GenerateMicrosoftExtensions(FilePicker);
            });

            var app = builder.Build();
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
            });

            app.Run();
        }
    }
}