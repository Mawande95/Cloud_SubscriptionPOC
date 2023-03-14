using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;


namespace CloudSubscriptionAPI
{
    public class ConfigureSwaggerOptions : IConfigureOptions<SwaggerGenOptions>
    {
        readonly IApiVersionDescriptionProvider provider;
        public ConfigureSwaggerOptions(IApiVersionDescriptionProvider provider) => this.provider = provider;
        public void Configure(SwaggerGenOptions options)
        {
           foreach(var desc in provider.ApiVersionDescriptions)
            {
                options.SwaggerDoc(
                    desc.GroupName, new Microsoft.OpenApi.Models.OpenApiInfo()
                    {
                        Title = $"CloudX API {desc.ApiVersion}",
                        Version = desc.ApiVersion.ToString()
                    });

            }
            var xmlCommandFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
            var cmlCommandsFullPath = Path.Combine(AppContext.BaseDirectory, xmlCommandFile);
            options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Description="",
                Name="Authorization",
                In = ParameterLocation.Header,
                Type= SecuritySchemeType.ApiKey,
                Scheme =    "Bearer"
            });
            options.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement()
           {
               {
                   new OpenApiSecurityScheme
                   {
                       Reference  = new OpenApiReference
                       {
                           Type = ReferenceType.SecurityScheme,
                           Id="Bearer"
                       },
                       Scheme = "oauth2",
                       Name = "Bearer",
                       In = ParameterLocation.Header

                   },
                   new List<string>()
               }
           });
        }


    }
}
