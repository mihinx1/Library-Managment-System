using System.Net;
using Microsoft.AspNetCore.Localization;

namespace IPSTemplate.AppServer.Helpers;

public static class HttpRequestExtensions
{
    private const string NullIpAddress = "::1";

    public static bool IsLocal(this HttpRequest req)
    {
        var connection = req.HttpContext.Connection;
        if (connection.RemoteIpAddress?.IsSet() == true)
        {
            // We have a remote address set up
            return connection.LocalIpAddress?.IsSet() == true
                // If local is same as remote, then we are local
                ? connection.RemoteIpAddress.Equals(connection.LocalIpAddress)
                // else we are remote if the remote IP address is not a loopback address
                : IPAddress.IsLoopback(connection.RemoteIpAddress);
        }

        return true;
    }

    public static string ToShortDescriptionString(this HttpRequest httpRequest)
    {
        return $"{httpRequest.Method} {httpRequest.Path}";
    }

    private static bool IsSet(this IPAddress address)
    {
        return address != null && address.ToString() != NullIpAddress;
    }

    public static ProviderCultureResult? GetCultureCookieValue(this HttpRequest request)
    {
        return CookieRequestCultureProvider.ParseCookieValue(
            request.Cookies.FirstOrDefault(p => p.Key == CookieRequestCultureProvider.DefaultCookieName).Value);
    }

    public static void SetCultureCookieValue(this HttpResponse response, RequestCulture requestCulture)
    {
        response.Cookies.Append(
            CookieRequestCultureProvider.DefaultCookieName,
            CookieRequestCultureProvider.MakeCookieValue(requestCulture));
    }
}
