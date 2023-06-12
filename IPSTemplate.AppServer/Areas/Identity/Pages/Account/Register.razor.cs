using Csla.Blazor;
using IPSBlazor.Services;
using IPSTemplate.AppServer.Helpers;
using IPSTemplate.BusinessLibrary.Interfaces;
using IPSTemplate.BusinessLibrary.StandardCollections;
using IPSTemplate.Dal.Models.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace IPSTemplate.AppServer.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public partial class Register : ComponentBase
    {
        public class Model
        {
            [Required(ErrorMessage = "Polje E-Mail je obvezno.")]
            [EmailAddress]
            [Display(Name = "E-mail")]
            public string? Email { get; set; }

            [Required(ErrorMessage = "Polje Geslo je obvezno.")]
            [DataType(DataType.Password)]
            [Display(Name = "Geslo")]
            public string? Password { get; set; }

            [Required(ErrorMessage = "Polje Potrdi geslo je obvezno.")]
            [DataType(DataType.Password)]
            [Compare(nameof(Password), ErrorMessage = "'Geslo' in 'Potrdi geslo' se ne ujemata.")]
            [Display(Name = "Potrdi geslo")]
            public string? ConfirmPassword { get; set; }

            [Required(ErrorMessage = "Polje Ime je obvezno.")]
            [Display(Name = "Ime")]
            public string? Name { get; set; }

            [Required(ErrorMessage = "Polje Priimek je obvezno.")]
            [Display(Name = "Priimek")]
            public string? Surname { get; set; }

            //[Required(ErrorMessage = "Polje Datum Rojstva je obvezno.")]
            //[Display(Name = "Datum Rojstva")]
            //public DateTime? BirthDate { get; set; }

            [Required(ErrorMessage = "Polje Nacionalnost je obvezno.")]
            [Display(Name = "Nacionalnost")]
            public string? Nationality { get; set; }

        }

        private UIActionResult? _actionResult;

        private bool HidePassword { get; set; } = true;

        private bool HideConfirmPassword { get; set; } = true;

        private string PageTitle { get; set; }

        private string ConfirmationMessage { get; set; }

        private string UrlInConfirm { get; set; }

        private string LinkInConfirm { get; set; }

        [Inject] private SignInManager<TEIdentityUser> SignInManager { get; set; } = default!;

        [Inject] private UserManager<TEIdentityUser> UserManager { get; set; } = default!;

        [Inject] private IUserService UserService { get; set; } = default!;

        [Inject] private NavigationManager NavigationManager { get; set; } = default!;

        [Inject] private ILogger<Register> Logger { get; set; } = default!;

        [Inject] private ViewModel<Model> ViewModel { get; set; } = default!;

        [Inject] NotificationService NotificationService { get; set; } = default!;

        [CascadingParameter]
        private Task<AuthenticationState> authenticationStateTask { get; set; } = default!;

        bool confirmationVisible = false;

        protected override async Task OnInitializedAsync()
        {
            await ViewModel.RefreshAsync(() => Task.FromResult(new Model()));
            await base.OnInitializedAsync();
        }
        private async Task RegisterUser()
        {
            // Create new user
            var user = new TEIdentityUser
            {
                UserName = ViewModel.Model.Email,
                Email = ViewModel.Model.Email,
                EmailConfirmed = true,
                Name = ViewModel.Model.Name,
                Surname = ViewModel.Model.Surname,
                //BirthDate = (DateTime)ViewModel.Model.BirthDate,
                Nationality = ViewModel.Model.Nationality
            };
            var result = await UserManager.CreateAsync(user, ViewModel.Model.Password);
            if (!result.Succeeded)
            {
                string errors = string.Join("; ", result.Errors.Select(e => e.Description));
                Logger.LogError("Error creating new user ({UnderlyingErrors})", errors);
                _actionResult = new UIActionResult
                {
                    Message = "Error creating new user.",
                    Status = IPSBlazor.Color.Danger
                };
                return;
            }

            await UserService.SaveUsersRoles(user.Id, new() { TEUserRole.Member });

            Logger.LogInformation("User created a new account with password.");

            confirmationVisible = true;

            // Send confirmation email
            //            try
            //            {
            //                string userId = await UserManager.GetUserIdAsync(user);
            //                var code = await UserManager.GenerateEmailConfirmationTokenAsync(user);
            //                code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
            //                string callbackUrl = NavigationManager.GetAbsoluteUriWithQueryParameters(
            //                    "/account/confirm-email",
            //                    new Dictionary<string, object?>
            //                    {
            //                    { "area", "Identity" },
            //                    { "userId", userId },
            //                    { "code", code }
            //                    });
            //#if DEBUG
            //                NavigationManager.NavigateTo(callbackUrl);
            //                return;
            //#else
            //                //await DISendConfirmationEmailCommand.ExecuteAsync(ViewModel.Model.Email, callbackUrl);
            //#endif
            //            }
            //            catch
            //            {
            //                await UserManager.DeleteAsync(user);
            //                _actionResult = new UIActionResult
            //                {
            //                    Message = "Error sending confirmation email. Please try again later.",
            //                    Status = IPSBlazor.Color.Danger
            //                };
            //                return;
            //            }

            //            // Redirect the user
            //            if (UserManager.Options.SignIn.RequireConfirmedAccount)
            //            {
            //                NavigationManager.NavigateTo(
            //                    "/account/register-confirmation",
            //                    new Dictionary<string, string?>
            //                    {
            //                        { "email", ViewModel.Model.Email }
            //                    });
            //            }
            //            else
            //            {
            //                await SignInManager.SignInAsync(user, isPersistent: false);
            //                if (!NavigationManager.TryGetQueryParameter("returnUrl", out string? returnUrl))
            //                {
            //                    returnUrl = "/";
            //                }
            //                NavigationManager.NavigateTo(returnUrl!);
            //            }



            return;
        }
    }
}
