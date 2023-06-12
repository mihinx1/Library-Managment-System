using System.Reflection;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.OpenApi.Models;

namespace IPSTemplate.AppServer.Middleware
{
    public static class SwaggerDependenciesMiddleware
    {
        /// <summary>
        /// Add Swagger middleware
        /// </summary>
        /// <remarks>
        /// See: https://github.com/drwatson1/AspNet-Core-REST-Service/wiki#documenting-api
        /// </remarks>
        public static IServiceCollection AddSwagger(this IServiceCollection services)
        {
            var provider = services.BuildServiceProvider()
                                   .GetRequiredService<IApiVersionDescriptionProvider>();

            // Register the Swagger generator, defining 1 or more Swagger documents
            services.AddSwaggerGen(c =>
            {
                c.OrderActionsBy(orderBy => orderBy.HttpMethod);
                //c.UseReferencedDefinitionsForEnums();
                foreach (var description in provider.ApiVersionDescriptions)
                {
                    c.SwaggerDoc(description.GroupName, new OpenApiInfo
                    {
                        Title = Constants.Swagger.ApiName,
                        //Version = description.ApiVersion.MajorVersion.ToString() + "." + description.ApiVersion.MinorVersion, //verzija iz AppSettings.DefaultApiVersion
                        Version = Constants.Swagger.Version, //verzija iz Constants.Swagger
                        Description = Constants.Swagger.Description,
                        Contact = new OpenApiContact
                        {
                            Name = Constants.Platform.Name,
                            Email = Constants.Platform.Email,
                            Url = new Uri(Constants.Platform.Url)
                        },
                    });
                }

                // Swagger 2.+ support
                var security = new Dictionary<string, IEnumerable<string>>
                {
                    {"Bearer", Array.Empty<string>()},
                };

                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                });
                c.AddSecurityRequirement(new OpenApiSecurityRequirement{
                    {
                        new OpenApiSecurityScheme{
                            Reference = new OpenApiReference{
                                Id = "Bearer", //The name of the previously defined security scheme.
                                Type = ReferenceType.SecurityScheme
                            }
                        },new List<string>()
                    }
                });

                // Set the comments path for the Swagger JSON and UI.
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                if (File.Exists(xmlPath))
                {
                    c.IncludeXmlComments(xmlPath);
                }
            });

            return services;
        }
    }
}
