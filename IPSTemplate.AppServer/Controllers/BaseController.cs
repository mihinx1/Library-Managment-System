using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace IPSTemplate.AppServer.Controllers;

public class BaseController : Controller
{
    protected ILogger? Logger { get; init; }
    protected AppSettings? AppSettings { get; init; }

    public BaseController()
    {
    }

    public BaseController(ILogger<BaseController> logger)
    {
        Logger = logger;
    }

    public BaseController(ILogger<BaseController> logger, IOptions<AppSettings> appSettings)
    {
        Logger = logger;
        AppSettings = appSettings?.Value;
    }
}
