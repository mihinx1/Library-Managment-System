using IPSTemplate.AppServer.Helpers;
using IPSTemplate.Dal.Models.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using System.Text;

namespace IPSTemplate.AppServer.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public partial class RegisterConfirmation : ComponentBase
    {
        private UIActionResult? _actionResult;

        [Inject] private UserManager<TEIdentityUser> UserManager { get; set; } = default!;

        [Inject] private NavigationManager NavigationManager { get; set; } = default!;

        [Inject] private ILogger<RegisterConfirmation> Logger { get; set; } = default!;

        protected override async Task OnInitializedAsync()
        {
            if (!NavigationManager.TryGetQueryParameter("email", out string? email))
            {
                NavigationManager.NavigateTo("/");
                return;
            }

            TEIdentityUser user = await UserManager.FindByEmailAsync(email);
            if (user == null)
            {
                _actionResult = new UIActionResult
                {
                    Message = $"Unable to load user with email '{email}'.",
                    Status = IPSBlazor.Color.Danger
                };
                return;
            }

            _actionResult = new UIActionResult
            {
                Message = "Please check your email to confirm your account.",
                Status = IPSBlazor.Color.Info
            };

            // Uncomment this method call to confirm the account if an email sender is not available
            //await DisplayConfirmAccountLink(user);

            await base.OnInitializedAsync();
        }

        private async Task DisplayConfirmAccountLink(TEIdentityUser user)
        {
            var userId = await UserManager.GetUserIdAsync(user);
            var code = await UserManager.GenerateEmailConfirmationTokenAsync(user);
            code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
            NavigationManager.TryGetQueryParameter("returnUrl", out string? returnUrl);
            string emailConfirmationUrl = NavigationManager.GetUriWithQueryParameters(
                "/account/confirm-email",
                new Dictionary<string, object?>
                {
                    { "area", "Identity" },
                    { "userId", userId },
                    { "code", code },
                    { "returnUrl", returnUrl }
                });

            _actionResult = new UIActionResult
            {
                Message = $@"This app does not currently have a real email sender registered, see <a href=""https://aka.ms/aspaccountconf"">these docs</a> for how to configure a real email sender. Normally this would be emailed: <a href=""{emailConfirmationUrl}""> Click here to confirm your account</a>.",
                Status = IPSBlazor.Color.Info
            };
        }
    }
}
