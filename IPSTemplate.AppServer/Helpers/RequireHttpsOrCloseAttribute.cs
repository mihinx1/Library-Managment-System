using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace IPSTemplate.AppServer.Helpers;

public class RequireHttpsOrCloseAttribute : RequireHttpsAttribute
{
    protected override void HandleNonHttpsRequest(AuthorizationFilterContext filterContext)
    {
        // Allow HTTP for local IP's
        if (!filterContext.HttpContext.Request.IsLocal())
            filterContext.Result = new StatusCodeResult(400);
    }
}
