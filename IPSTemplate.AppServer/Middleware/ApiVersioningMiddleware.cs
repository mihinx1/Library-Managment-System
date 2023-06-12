using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Versioning;

namespace IPSTemplate.AppServer.Middleware
{
    public static class ApiVersioningMiddleware
    {
        public static void AddApiVersioning(this IServiceCollection services, IConfiguration configuration) =>
            services
                .AddApiVersioning(apiVersioningOptions =>
                {
                    var apiVersion = new Version(Convert.ToString(configuration["DefaultApiVersion"]));
                    apiVersioningOptions.DefaultApiVersion = new ApiVersion(apiVersion.Major, apiVersion.Minor);

                    // API versions will be defined in the HTTP header
                    apiVersioningOptions.ApiVersionReader = new HeaderApiVersionReader("api-version");

                    // Set AssumeDefaultVersionWhenUnspecified to true for our default API version to be activated
                    apiVersioningOptions.AssumeDefaultVersionWhenUnspecified = true;

                    // Responses from our API endpoints will carry a header telling our clients which versions are supported or deprecated
                    apiVersioningOptions.ReportApiVersions = true;

                    // Include only API controllers and ignore MVC controllers
                    apiVersioningOptions.UseApiBehavior = true;

                    // Select the API version based on DefaultApiVersion if the HTTP header is not specified
                    apiVersioningOptions.ApiVersionSelector = new DefaultApiVersionSelector(apiVersioningOptions);
                })
                .AddVersionedApiExplorer();
    }
}
