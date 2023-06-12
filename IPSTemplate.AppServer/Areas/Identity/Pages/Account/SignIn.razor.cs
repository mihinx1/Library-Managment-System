using Csla.Blazor;
using IPSTemplate.AppServer.Helpers;
using IPSTemplate.Dal.Models.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace IPSTemplate.AppServer.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public partial class SignIn : ComponentBase
    {
        public class Model
        {
            [Required]
            //[Display(ResourceType = typeof(IPSDiva.Ui.Blazor.Resources.Resources), Name = "Login_Email")]
            [Display(Name = "Email")]
            public string? Username { get; set; }

            [Required]
            [DataType(DataType.Password)]
            //[Display(ResourceType = typeof(IPSDiva.Ui.Blazor.Resources.Resources), Name = "Login_Password")]
            [Display(Name = "Geslo")]
            public string? Password { get; set; }

            //[Display(ResourceType = typeof(IPSDiva.Ui.Blazor.Resources.Resources), Name = "Login_RememberMe")]
            public bool RememberMe { get; set; }
        }

        private UIActionResult? _actionResult;

        private bool HidePassword { get; set; } = true;

        [Inject] private UserManager<TEIdentityUser> UserManager { get; set; } = default!;

        [Inject] private NavigationManager NavigationManager { get; set; } = default!;

        [Inject] private IDataProtectionProvider DataProtectionProvider { get; set; } = default!;

        [Inject] private ILogger<SignIn> Logger { get; set; } = default!;

        [Inject] private ViewModel<Model> ViewModel { get; set; } = default!;

        //[Inject] protected IStringLocalizer<IPSDiva.Ui.Blazor.Resources.Resources> Localizer { get; set; } = default!;

        protected bool SigningIn { get; set; }

        protected override async Task OnInitializedAsync()
        {
            await ViewModel.RefreshAsync(() => Task.FromResult(new Model()));
            await base.OnInitializedAsync();
        }

        private async Task SignInUser()
        {
            SigningIn = true;

            //var user = await UserManager.FindByEmailAsync(ViewModel.Model.Email);
            var user = await UserManager.FindByNameAsync(ViewModel.Model.Username);
            if (user is not null && user.Active && await UserManager.CheckPasswordAsync(user, ViewModel.Model.Password))
            {
                var token = await UserManager.GenerateUserTokenAsync(user, TokenOptions.DefaultProvider, "SignIn");
                var data = $"{user.Id}|{token}";
                var parsedQuery = System.Web.HttpUtility.ParseQueryString(new Uri(NavigationManager.Uri).Query);
                var returnUrl = parsedQuery["returnUrl"];
                if (!string.IsNullOrWhiteSpace(returnUrl))
                {
                    data += $"|{returnUrl}";
                }

                var protector = DataProtectionProvider.CreateProtector("SignIn");
                var pdata = protector.Protect(data);

                NavigationManager.NavigateTo("/account/sign-in/redirect?t=" + pdata, forceLoad: true);
            }
            else if (user is not null && !user.Active)
            {
                _actionResult = new UIActionResult
                {
                    Message = "User deactivated",
                    Status = IPSBlazor.Color.Danger
                };
                SigningIn = false;
            }
            else
            {
                _actionResult = new UIActionResult
                {
                    Message = "Invalid credentials",
                    Status = IPSBlazor.Color.Danger
                };
                SigningIn = false;
            }

        }
    }
}
