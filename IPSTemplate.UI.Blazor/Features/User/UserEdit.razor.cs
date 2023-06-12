using Core.DAL.Infrastructure;
using IPSBlazor.Components;
using IPSBlazor.Services;
using IPSTemplate.BusinessLibrary.BO.Identity.User;
using IPSTemplate.BusinessLibrary.Interfaces;
using IPSTemplate.Dal.Models.Identity;
using IPSTemplate.UI.Blazor.Base;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace IPSTemplate.UI.Blazor.Features.User
{
    public partial class UserEdit : EditView<TEUserBO, Entity>
    {
        [Inject] protected IDataPortalFactory DataPortalFactory { get; set; } = default!;

        [Inject] protected IUserService UserService { get; set; } = default!;
        [Inject] public AuthenticationStateProvider AuthenticationStateProvider { get; set; } = default!;
        [Parameter] public EventCallback ItemSaved { get; set; }
        [Inject] public UserManager<TEIdentityUser> UserManager { get; set; } = default!;
        private ClaimsPrincipal user = default!;
        public Guid UserId = default!;
        public IUserService IUserService = default!;
        protected IPSComboBox<Guid, TEUserInfo> cbxUser = default!;
        [Inject] NotificationService NotificationService { get; set; }
        private bool HidePassword1 { get; set; } = true;
        private bool HidePassword2 { get; set; } = true;
        public string? Password1 { get; set; }
        public string? Password2 { get; set; }

        private async Task SetNewPassword()
        {
            if (Password1 != null && Password2 != null)
            {
                if (Password1 == Password2)
                {
                    var auth = await AuthenticationStateProvider.GetAuthenticationStateAsync();
                    var user = auth.User;
                    var curUser = await UserManager.GetUserAsync(user);
                    var userId = curUser.Id;
                    await UserService.SetUserPassword(curUser.Id, Password1);
                    NotificationService.ShowSuccess("Uspešno posodobljeno geslo.");
                    Password1 = Password2 = "";
                }
                else
                {
                    NotificationService.ShowError("Gesli se ne ujemata. Prosim preverite.");
                }
            }
        }

        protected override async Task OnInitializedAsync()
        {
            var auth = await AuthenticationStateProvider.GetAuthenticationStateAsync();
            user = auth.User;
            UserId = Guid.Parse(UserManager.GetUserId(user));
            await base.OnInitializedAsync();
            ViewModel.Model.Id = UserId;
            await ViewModel.RefreshAsync(() => TEUserGetCommand.GetUserAsync(UserId, DataPortalFactory));
        }
    }
}
