//using Core.DAL.Infrastructure;
//using Core.Library.Base;
//using Csla;
//using IPSTemplate.BusinessLibrary.Interfaces;
//using Microsoft.AspNetCore.Hosting.Server;
//using Microsoft.AspNetCore.Hosting.Server.Features;
//using Microsoft.Extensions.Logging;

//namespace IPSTemplate.BusinessLibrary.BO.Identity.User
//{
//    [Serializable]
//    public class TEUserSendPasswordResetLinkCommand : CslaCommandBase<TEUserSendPasswordResetLinkCommand, Entity>
//    {
//        #region Properties

//        public static readonly PropertyInfo<TEUserForgottenPasswordBO> ForgottenPasswordDataProperty = RegisterProperty<TEUserForgottenPasswordBO>(p => p.ForgottenPasswordData);
//        public TEUserForgottenPasswordBO ForgottenPasswordData
//        {
//            get => ReadProperty(ForgottenPasswordDataProperty);
//            set => LoadProperty(ForgottenPasswordDataProperty, value);
//        }

//        public static readonly PropertyInfo<string> LanguageProperty = RegisterProperty<string>(p => p.Language);
//        public string Language
//        {
//            get => ReadProperty(LanguageProperty);
//            set => LoadProperty(LanguageProperty, value);
//        }

//        public static readonly PropertyInfo<bool> SuccessProperty = RegisterProperty<bool>(p => p.Success);
//        public bool Success
//        {
//            get => ReadProperty(SuccessProperty);
//            set => LoadProperty(SuccessProperty, value);
//        }

//        public static readonly PropertyInfo<string> ErrorMessageProperty = RegisterProperty<string>(p => p.ErrorMessage);
//        public string ErrorMessage
//        {
//            get => ReadProperty(ErrorMessageProperty);
//            set => LoadProperty(ErrorMessageProperty, value);
//        }

//        #endregion

//        #region Client-side methods

//        public static (bool Success, string ErrorMessage) SendPasswordResetLink(TEUserForgottenPasswordBO forgottenPasswordData, IDataPortalFactory factory)
//        {
//            var cmd = factory.GetPortal<TEUserSendPasswordResetLinkCommand>().Create();
//            cmd.ForgottenPasswordData = forgottenPasswordData;
//            cmd.Language = System.Globalization.CultureInfo.CurrentCulture.TwoLetterISOLanguageName;
//            cmd = factory.GetPortal<TEUserSendPasswordResetLinkCommand>().Execute(cmd);
//            return (cmd.Success, cmd.ErrorMessage);
//        }

//        public static async Task<(bool Success, string ErrorMessage)> SendPasswordResetLinkAsync(TEUserForgottenPasswordBO forgottenPasswordData, IDataPortalFactory factory)
//        {
//            var cmd = await factory.GetPortal<TEUserSendPasswordResetLinkCommand>().CreateAsync();
//            cmd.ForgottenPasswordData = forgottenPasswordData;
//            cmd.Language = System.Globalization.CultureInfo.CurrentCulture.TwoLetterISOLanguageName;
//            cmd = await factory.GetPortal<TEUserSendPasswordResetLinkCommand>().ExecuteAsync(cmd);
//            return (cmd.Success, cmd.ErrorMessage);
//        }

//        #endregion

//        #region Server-side methods

//        [Execute]
//        protected async Task Execute([Inject] IUserService userService, [Inject] ILogger logger, [Inject] IServer server, [Inject] IDataPortalFactory factory)
//        {
//            try
//            {
//                var user = await userService.GetUserByEmailAsync(ForgottenPasswordData.UserEmail);
//                if (user == null)
//                    throw new Exception("User not found");
//                if (!user.Active)
//                    throw new Exception("User deactivated");

//                //var userGuid = new Guid(user.Id);
//                var token = await userService.SetPasswordResetToken(user.Id);

//                //getting server address
//                var addresses = server.Features.Get<IServerAddressesFeature>();
//                var address = addresses.Addresses.FirstOrDefault();
//                string url = $"{address}account/reset-password/{token}";
//                var templatePrams = new Dictionary<string, string>();
//                templatePrams.Add("PasswordResetLink", url);

//                var msg = await DIMessageBO.NewItemAsync(factory);
//                msg.UserID = user.Id;
//                msg.MessageType = DIMessageType.Email;
//                msg.RecipientEmailAddress = ForgottenPasswordData.UserEmail;

//                msg = await msg.SendMessageAsync("pass_reset", Language, templatePrams, factory);

//                Success = msg.Status != DIMessageStatus.FailedNoRetry;
//                if (!Success)
//                    throw new Exception("Error sending password reset link!");
//            }
//            catch (Exception e)
//            {
//                logger?.LogError(e, $"Error sending password reset link to {ForgottenPasswordData.UserEmail}");
//                Success = false;
//                ErrorMessage = e.Message;
//            }
//        }

//        #endregion
//    }
//}
