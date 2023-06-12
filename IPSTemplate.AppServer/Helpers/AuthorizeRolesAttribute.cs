using Microsoft.AspNetCore.Authorization;

namespace IPSTemplate.AppServer.Helpers;

public class AuthorizeRolesAttribute : AuthorizeAttribute
{
    public AuthorizeRolesAttribute(params string[] roles)
    {
        if (EnvironmentHelper.IsDevelopment || EnvironmentHelper.IsStaging)
        {
            roles = roles.Select(p => p + "TEST").ToArray();
        }
        Roles = string.Join(",", roles);
    }
}
