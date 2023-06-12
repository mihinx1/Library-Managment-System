using Microsoft.AspNetCore.Mvc.ApiExplorer;

namespace IPSTemplate.AppServer.Middleware
{
    public static class SwaggerMiddleware
    {
        /// <summary>
        /// Use swagger UI and endpoint
        /// </summary>
        /// <remarks>
        /// See: https://github.com/drwatson1/AspNet-Core-REST-Service/wiki#documenting-api
        /// </remarks>
        public static IApplicationBuilder UseSwaggerWithOptions(
            this IApplicationBuilder app, IApiVersionDescriptionProvider apiVersionDescriptionProvider)
        {
            // Enable middleware to serve generated Swagger as a JSON endpoint.
            SwaggerBuilderExtensions.UseSwagger(app);

            app.UseSwagger();

            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.), 
            // specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c =>
            {
                // Uncomment this line if you want to access the Swagger UI as http://localhost:5000
                // c.RoutePrefix = string.Empty;    
                c.DocumentTitle = Constants.Swagger.ApiName;
                foreach (var item in apiVersionDescriptionProvider.ApiVersionDescriptions)
                {
                    c.SwaggerEndpoint($"/swagger/{item.GroupName}/swagger.json", item.GroupName);
                }
            });

            return app;
        }
    }
}
