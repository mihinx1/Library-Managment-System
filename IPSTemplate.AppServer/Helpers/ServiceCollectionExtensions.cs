using System.Net;
using Hellang.Middleware.ProblemDetails;
using Microsoft.AspNetCore.Mvc;

namespace IPSTemplate.AppServer.Helpers;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddHttpClient<TClient, TImplementation>(
        this IServiceCollection services,
        AppSettings appSettings,
        bool useDangerousAcceptAnyServerCertificateValidator = false)
        where TClient : class
        where TImplementation : class, TClient =>
            services
                .AddHttpClient<TClient, TImplementation>(client =>
                    client.Timeout = TimeSpan.FromSeconds(int.TryParse(appSettings.HttpClientTimeout, out int timeout) ? timeout : 60))
                .ConfigurePrimaryHttpMessageHandler(p => new DefaultHttpClientHandler(useDangerousAcceptAnyServerCertificateValidator))
                .Services;

    public static IServiceCollection AddProblemDetailsExtended(this IServiceCollection services) =>
        services.AddProblemDetails(config =>
        {
            config.Map<ArgumentException>(ex => new ProblemDetails
            {
                Title = "Bad Request",
                Detail = ex.Message,
                Status = StatusCodes.Status400BadRequest,
                Type = "https://httpstatuses.com/400"
            });
            config.Map<KeyNotFoundException>(ex => new ProblemDetails
            {
                Title = "Not Found",
                Detail = ex.Message,
                Status = StatusCodes.Status404NotFound,
                Type = "https://httpstatuses.com/404"
            });
            config.Map<Core.DAL.Infrastructure.DataNotFoundException>(ex => new ProblemDetails
            {
                Title = "Data Not Found",
                Detail = ex.Message,
                Status = StatusCodes.Status404NotFound,
                Type = "https://httpstatuses.com/404"
            });
            config.Map<Core.DAL.Infrastructure.ConcurrencyException>(ex => new ProblemDetails
            {
                Title = "Concurrency Exception",
                Detail = ex.Message,
                Status = StatusCodes.Status409Conflict,
                Type = "https://httpstatuses.com/409"
            });
        });

    public class DefaultHttpClientHandler : HttpClientHandler
    {
        public DefaultHttpClientHandler(bool useDangerousAcceptAnyServerCertificateValidator)
        {
            AutomaticDecompression =
                DecompressionMethods.Brotli |
                DecompressionMethods.Deflate |
                DecompressionMethods.GZip;

            if (useDangerousAcceptAnyServerCertificateValidator)
                ServerCertificateCustomValidationCallback = DangerousAcceptAnyServerCertificateValidator;
        }
    }
}
