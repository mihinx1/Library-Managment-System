namespace IPSTemplate.UI.Blazor.Helpers;

internal class AuthorizationHelper
{
    public static string RolesString(params string[] roles) => string.Join(',', roles);
}
