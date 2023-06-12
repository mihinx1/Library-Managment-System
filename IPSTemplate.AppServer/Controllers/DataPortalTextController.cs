using Csla;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace IPSTemplate.AppServer.Controllers;

[ApiController]
[ApiVersion("1.0")]
[Route("[controller]")]
[ApiExplorerSettings(IgnoreApi = true)]
[Authorize]
public class DataPortalTextController : DataPortalController
{
    public DataPortalTextController(ApplicationContext applicationContext) : base(applicationContext)
    {
        UseTextSerialization = true;
    }
}
