using Csla;
using Csla.Channels.Http;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Microsoft.Extensions.DependencyInjection;

namespace IPSTemplate.UI.Blazor.Helpers
{
    public static class CslaHttpClientMiddleware
    {
        public static IServiceCollection RegisterCslaHttpClient(this IServiceCollection services, string uri)
        {
#if DEBUG
            services.AddHttpClient<CslaHttpClient>(client => client.BaseAddress = new Uri(uri))
                .AddHttpMessageHandler(sp =>
                {
                    var handler = sp.GetRequiredService<CustomAuthorizationMessageHandler>()
                        .ConfigureHandler(
                            authorizedUrls: new[] { uri } // The URI used by the Server project.
                        );

                    return handler;
                });
#else
            services.AddHttpClient<CslaHttpClient>(client => client.BaseAddress = new Uri(uri))
                .AddHttpMessageHandler<CustomAuthorizationMessageHandler>();
#endif
            return services;
        }
    }

    public class CslaHttpClient
    {
        public HttpClient Client { get; private set; }

        public CslaHttpClient(HttpClient httpClient)
        {
            //httpClient.DefaultRequestHeaders.Add("Accept", "application/vnd.github.v3+json");
            //httpClient.DefaultRequestHeaders.Add("User-Agent", "HttpClientFactory-Sample");
            httpClient.Timeout = new TimeSpan(0, 3, 0);
            Client = httpClient;
        }
    }

    public class CslaDataPortalHttpProxy : HttpProxy
    {
        private readonly CslaHttpClient _httpClient;

        public CslaDataPortalHttpProxy(CslaHttpClient httpClient, ApplicationContext appContext) : base(appContext, httpClient.Client, null)
        {
            _httpClient = httpClient;
        }

        protected override HttpClient GetHttpClient()
        {
            return _httpClient.Client;
        }
    }

    /// <summary>
    /// A <see cref="DelegatingHandler"/> that attaches access tokens to outgoing <see cref="HttpResponseMessage"/> instances.
    /// Access tokens will only be added when the request URI is within the application's base URI.
    /// </summary>
    public class CustomAuthorizationMessageHandler : AuthorizationMessageHandler
    {
        /// <summary>
        /// Custom message handler
        /// </summary>
        /// <param name="provider">The <see cref="IAccessTokenProvider"/> to use for requesting tokens.</param>
        /// <param name="navigationManager">The <see cref="NavigationManager"/> used to compute the base address.</param>
        public CustomAuthorizationMessageHandler(IAccessTokenProvider provider, NavigationManager navigationManager)
            : base(provider, navigationManager)
        {
#if !DEBUG
            // For RELEASE we can set the BaseUri here
            ConfigureHandler(new[] { navigationManager.BaseUri });
#endif
        }

        /// <inheritdoc />
        protected override async Task<HttpResponseMessage?> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            try
            {
                return await base.SendAsync(request, cancellationToken);
            }
            catch (AccessTokenNotAvailableException ex)
            {
                ex.Redirect();
                return null;
            }
        }
    }
}
