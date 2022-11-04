﻿using System.Collections.Generic;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Swagger;
using SwashBuckle.AspNetCore.MicrosoftExtensions.Extensions;
using SwashBuckle.AspNetCore.MicrosoftExtensions.VendorExtensionEntities;

namespace TestApi
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc(x => x.EnableEndpointRouting = false);

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1" });
                c.GenerateMicrosoftExtensions(FilePicker);
            });
        }

        private FilePickerCapabilityModel FilePicker =>
            new FilePickerCapabilityModel
            (
                new FilePickerOperationModel("InitialOperation", null),
                new FilePickerOperationModel("BrowsingOperation", new Dictionary<string, string> { { "Id", "Id" } }),
                "Name",
                "IsFolder",
                "MediaType"
            );


        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure
        (
            IApplicationBuilder app
        )
        {
            app.UseMvc();
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
            });
        }
    }
}