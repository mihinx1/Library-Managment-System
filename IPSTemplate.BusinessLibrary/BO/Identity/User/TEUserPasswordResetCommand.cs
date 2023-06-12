using Core.DAL.Infrastructure;
using Core.Library.Base;
using Csla;
using IPSTemplate.BusinessLibrary.Interfaces;
using IPSTemplate.Dal.Models.Identity;
using Microsoft.Extensions.Logging;

namespace IPSTemplate.BusinessLibrary.BO.Identity.User
{
    [Serializable]
    public class TEUserPasswordResetCommand : CslaCommandBase<TEUserPasswordResetCommand, Entity>
    {
        #region Properties

        public static readonly PropertyInfo<TEUserPasswordResetBO> ResetDataProperty = RegisterProperty<TEUserPasswordResetBO>(p => p.ResetData);
        public TEUserPasswordResetBO ResetData
        {
            get => ReadProperty(ResetDataProperty);
            set => LoadProperty(ResetDataProperty, value);
        }

        public static readonly PropertyInfo<bool> SuccessProperty = RegisterProperty<bool>(p => p.Success);
        public bool Success
        {
            get => ReadProperty(SuccessProperty);
            set => LoadProperty(SuccessProperty, value);
        }

        public static readonly PropertyInfo<string> ErrorMessageProperty = RegisterProperty<string>(p => p.ErrorMessage);
        public string ErrorMessage
        {
            get => ReadProperty(ErrorMessageProperty);
            set => LoadProperty(ErrorMessageProperty, value);
        }

        #endregion

        #region Client-side methods

        public static (bool Success, string ErrorMessage) ResetPassword(TEUserPasswordResetBO resetData, IDataPortalFactory factory)
        {
            var cmd = factory.GetPortal<TEUserPasswordResetCommand>().Create();
            cmd.ResetData = resetData;
            cmd = factory.GetPortal<TEUserPasswordResetCommand>().Execute(cmd);
            return (cmd.Success, cmd.ErrorMessage);
        }

        public static async Task<(bool Success, string ErrorMessage)> ResetPasswordAsync(TEUserPasswordResetBO resetData, IDataPortalFactory factory)
        {
            var cmd = await factory.GetPortal<TEUserPasswordResetCommand>().CreateAsync();
            cmd.ResetData = resetData;
            cmd = await factory.GetPortal<TEUserPasswordResetCommand>().ExecuteAsync(cmd);
            return (cmd.Success, cmd.ErrorMessage);
        }

        #endregion

        #region Server-side methods

        [Execute]
        protected async Task Execute([Inject] IUserService userService, [Inject] ILogger logger)
        {
            try
            {
                TEIdentityUser user = null;
                if (!string.IsNullOrEmpty(ResetData.ResetToken))
                {
                    user = await userService.GetUserByResetToken(ResetData.ResetToken);
                    if (user == null)
                        throw new Exception("Reset password link is invalid");

                }
                else
                {
                    user = await userService.GetUserByIdAsync(ResetData.UserId);
                    if (user == null)
                        throw new Exception("User not found");
                    if (!user.Active)
                        throw new Exception("User deactivated");
                    var passwordMatch = await userService.PasswordMatchesAsync(ResetData.UserId, ResetData.CurrentPassword);
                    if (!passwordMatch)
                        throw new Exception("Old Password is invalid");
                }
                if (user != null)
                {
                    await userService.SetUserPassword(user.Id, ResetData.NewPassword);
                    Success = true;
                }
            }
            catch (Exception e)
            {
                logger?.LogError(e, "Error resetting password for " + (string.IsNullOrEmpty(ResetData.ResetToken) ? ("User " + ResetData.UserId.ToString()) : ("Token " + ResetData.ResetToken)));
                Success = false;
                ErrorMessage = e.Message;
            }
        }

        #endregion
    }
}
