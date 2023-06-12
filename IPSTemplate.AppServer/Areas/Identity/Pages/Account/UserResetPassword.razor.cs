//using Csla;
//using IPSBlazor.Services;
//using IPSTemplate.BusinessLibrary.Interfaces;
//using IPSTemplate.Dal.Models.Identity;
//using Microsoft.AspNetCore.Components.Authorization;
//using Microsoft.AspNetCore.Identity;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace IPSTemplate.UI.Blazor.Features.User
//{
//    public partial class UserResetPassword
//    {
//        [Inject] IUserService UserService { get; set; }
//        [Inject] AuthenticationStateProvider AuthenticationStateProvider { get; set; }
//        [Inject] UserManager<TEIdentityUser> UserManager { get; set; }
//        [Inject] NotificationService NotificationService { get; set; }
//        private bool HidePassword1 { get; set; } = true;
//        private bool HidePassword2 { get; set; } = true;
//        public string? Password1 { get; set; }
//        public string? Password2 { get; set; }

//        private async Task SetNewPassword()
//        {
//            if (Password1 != null && Password2 != null)
//            {
//                if (Password1 == Password2)
//                {
//                    var auth = await AuthenticationStateProvider.GetAuthenticationStateAsync();
//                    var user = auth.User;
//                    var curUser = await UserManager.GetUserAsync(user);
//                    var userId = curUser.Id;
//                    await UserService.SetUserPassword(curUser.Id, Password1);
//                    NotificationService.ShowSuccess("Uspešno posodobljeno geslo.");
//                    Password1 = Password2 = "";
//                }
//                else
//                {
//                    NotificationService.ShowError("Gesli se ne ujemata. Prosim preverite.");
//                }
//            }
//        }
//    }
//}
