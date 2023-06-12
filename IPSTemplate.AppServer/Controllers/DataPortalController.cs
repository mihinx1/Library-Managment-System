using Csla;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace IPSTemplate.AppServer.Controllers;

[ApiController]
[ApiVersion("1.0")]
[Route("[controller]")]
[AllowAnonymous]
[ApiExplorerSettings(IgnoreApi = true)]
public class DataPortalController : Csla.Server.Hosts.HttpPortalController
{
    public DataPortalController(ApplicationContext applicationContext) : base(applicationContext)
    {
    }

    // This override exists for demo purposes only, normally you
    // wouldn't implement this code.
    [HttpPost]
    public override Task PostAsync([FromQuery] string operation)
    {
        return base.PostAsync(operation);
    }

    // Implementing a GET is totally optional, but is nice
    // for quick diagnosis as to whether a service is running.
    [HttpGet]
    public async Task<ActionResult<string>> GetAsync()
    {
        try
        {
            return await Task.FromResult("Hello world!");
        }
        catch (Exception ex)
        {
            return ex.ToString();
        }
    }
}
