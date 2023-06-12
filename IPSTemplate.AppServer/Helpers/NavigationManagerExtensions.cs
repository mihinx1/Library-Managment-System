using System.Net;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Primitives;

namespace IPSTemplate.AppServer.Helpers;

public static class NavigationManagerExtensions
{
    public static bool TryGetQueryParameter<T>(this NavigationManager navigationManager, string key, out T? value)
    {
        Uri url = navigationManager.ToAbsoluteUri(navigationManager.Uri);
        Dictionary<string, StringValues> query = QueryHelpers.ParseQuery(WebUtility.HtmlDecode(url.Query));

        if (query.TryGetValue(key, out var queryParameter))
        {
            return TConverter.TryChangeType(queryParameter, out value);
        }

        value = default;
        return false;
    }

    public static void NavigateTo(this NavigationManager navigationManager, string uri, IDictionary<string, string?> queryString)
    {
        uri = QueryHelpers.AddQueryString(uri, queryString);
        navigationManager.NavigateTo(uri);
    }

    public static string GetAbsoluteUriWithQueryParameters(
        this NavigationManager navigationManager,
        string uri,
        IReadOnlyDictionary<string, object?> queryString)
    {
        string absoluteUri = new Uri(new Uri(navigationManager.BaseUri), uri).ToString();
        return navigationManager.GetUriWithQueryParameters(absoluteUri, queryString);
    }
}
