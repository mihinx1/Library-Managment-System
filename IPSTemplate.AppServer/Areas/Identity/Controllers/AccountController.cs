using System.Security.Claims;
using Csla;
using IPSTemplate.BusinessLibrary.Interfaces;
using IPSTemplate.Dal.Models.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace IPSTemplate.AppServer.Areas.Identity.Account.Controllers;

[Route("[controller]")]
public class AccountController : ControllerBase
{
    private readonly UserManager<TEIdentityUser> _userManager;
    private readonly SignInManager<TEIdentityUser> _signInManager;
    private readonly IDataProtector _dataProtector;
    private readonly ApplicationContext _appContext;
    private readonly IDataPortalFactory _dataPortalFactory;

    public AccountController(
        IDataProtectionProvider dataProtectionProvider,
        UserManager<TEIdentityUser> userManager,
        SignInManager<TEIdentityUser> signInManager,
        ApplicationContext appContext,
        IDataPortalFactory dataPortalFactory)
    {
        _dataProtector = dataProtectionProvider.CreateProtector("SignIn");
        _userManager = userManager;
        _signInManager = signInManager;
        _appContext = appContext;
        _dataPortalFactory = dataPortalFactory;
    }

    [HttpGet("sign-in/redirect")]
    public async Task<IActionResult> SignIn(string t)
    {
        var data = _dataProtector.Unprotect(t);
        var parts = data.Split('|');
        var identityUser = await _userManager.FindByIdAsync(parts[0]);
        var isTokenValid = await _userManager.VerifyUserTokenAsync(identityUser, TokenOptions.DefaultProvider, "SignIn", parts[1]);

        if (isTokenValid && identityUser.Active)
        {
            await _signInManager.SignInAsync(identityUser, true);

            //creation and enrichment of Csla user -> add additional user properties needed in runtime
            var claims = await _userManager.GetClaimsAsync(identityUser);
            var identityClaim = claims.FirstOrDefault(c => c.Type == ClaimTypes.Sid);
            var supplierClaim = claims.FirstOrDefault(c => c.Type == "SupplierId");

            if (identityClaim == null || identityClaim.Value != identityUser.Id.ToString())
            {
                if (identityClaim != null)
                    await _userManager.RemoveClaimAsync(identityUser, identityClaim);

                identityClaim = new Claim(ClaimTypes.Sid, identityUser.Id.ToString());
                await _userManager.AddClaimAsync(identityUser, identityClaim);
            }

            _appContext.User = await _signInManager.CreateUserPrincipalAsync(identityUser);

            // Set preferred language and formatting
            //var culture = await DICultureSettings.GetGlobalSettingStringAsync(DICultureSettings.CultureSettings, _dataPortalFactory)
            //    ?? HttpContext.Request.GetCultureCookieValue()?.Cultures.SingleOrDefault().Value;

            //var uiCulture = HttpContext.Request.GetCultureCookieValue()?.UICultures.SingleOrDefault().Value
            //    ?? await DICultureSettings.GetUserSettingStringAsync(DICultureSettings.UICultureSettings, identityUser.Id.ToString(), _dataPortalFactory);

            //if (culture is not null || uiCulture is not null)
            //{
            //    RequestCulture requestCulture = culture is not null && uiCulture is not null
            //        ? new(culture, uiCulture)
            //        : culture is not null
            //            ? new(new CultureInfo(culture), CultureInfo.CurrentUICulture)
            //            : new(CultureInfo.CurrentCulture, new CultureInfo(uiCulture));

            //    if (!CultureInfo.CurrentCulture.Equals(requestCulture.Culture) ||
            //        !CultureInfo.CurrentUICulture.Equals(requestCulture.UICulture))
            //    {
            //        HttpContext.Response.SetCultureCookieValue(requestCulture);
            //    }

            //    if (uiCulture is not null && !DICultureSettings.UserSettingExists(DICultureSettings.UICultureSettings, identityUser.Id.ToString(), _dataPortalFactory))
            //    {
            //        await DICultureSettings.SetUserSettingStringAsync(DICultureSettings.UICultureSettings, uiCulture, identityUser.Id.ToString(), _dataPortalFactory);
            //    }
            //}

            return parts.Length == 3 && Url.IsLocalUrl(parts[2]) ? Redirect(parts[2]) : Redirect("/");
        }
        else
        {
            return Unauthorized("STOP!");
        }
    }

    [Authorize]
    [HttpGet("sign-out")]
    public new async Task<IActionResult> SignOut()
    {
        await _signInManager.SignOutAsync();

        _appContext.User = new ClaimsPrincipal(new ClaimsIdentity());

        return Redirect("/account/sign-in");
    }

    [HttpGet("set-password")]
    public async Task SetPassword([FromQuery] Guid userId, [FromQuery] string password, [FromServices] IUserService userService)
    {
        await userService.ResetUserPassword(userId, password);
    }
}
