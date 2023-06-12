using System.Net;
using Microsoft.AspNetCore.Authorization;
using NetTools;

namespace IPSTemplate.AppServer.Helpers;

public class IpAuthorizationRequirement : IAuthorizationRequirement
{
    /// <summary>
    /// Creates a new IpAuthorizationRequirement
    /// </summary>
    /// <param name="ipAddresses">Single IP address or comma-delimited list</param>
    public IpAuthorizationRequirement(string ipAddresses)
    {
        IPAddresses = ipAddresses;
    }

    public string IPAddresses { get; }
}

public class IpAuthorizationHandler : AuthorizationHandler<IpAuthorizationRequirement>
{
    private readonly ILogger<IpAuthorizationHandler> _logger;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public IpAuthorizationHandler(IHttpContextAccessor httpContextAccessor, ILogger<IpAuthorizationHandler> logger)
    {
        _httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
        _logger = logger;
    }

    private HttpContext HttpContext => _httpContextAccessor.HttpContext!;

    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, IpAuthorizationRequirement requirement)
    {
        if (string.IsNullOrEmpty(requirement.IPAddresses))
        {
            // No IP is allowed if the list is empty
            _logger.LogWarning("No allowed IP addresses defined");
            return Task.CompletedTask;
        }

        if (requirement.IPAddresses == "*")
        {
            context.Succeed(requirement);
            return Task.CompletedTask;
        }

        var allowedIps = requirement.IPAddresses.Split(',');
        IPAddress clientIp = HttpContext.Connection.RemoteIpAddress!;

        foreach (var item in allowedIps)
        {
            var ip = item.Trim();
            if (IPAddress.TryParse(ip, out var allowedIp))
            {
                if (clientIp.Equals(allowedIp))
                {
                    context.Succeed(requirement);
                }
            }
            else if (IPAddressRange.TryParse(ip, out var allowedIpRange))
            {
                if (allowedIpRange.Contains(clientIp))
                {
                    context.Succeed(requirement);
                }
            }
        }

        if (!context.HasSucceeded)
        {
            _logger.LogWarning("An incoming request from IP address '{ClientIP}' was blocked", clientIp);
        }

        return Task.CompletedTask;
    }
}
