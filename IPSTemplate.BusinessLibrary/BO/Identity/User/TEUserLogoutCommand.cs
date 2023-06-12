using Core.DAL.Infrastructure;
using Core.Library.Base;
using Csla;
using IPSTemplate.BusinessLibrary.Interfaces;
using IPSTemplate.Dal.Models.Identity;
using Microsoft.Extensions.Logging;

namespace IPSTemplate.BusinessLibrary.BO.Identity.User
{
    [Serializable]
    public class TEUserLogoutCommand : CslaCommandBase<TEUserLogoutCommand, Entity>
    {
        #region Properties

        public static readonly PropertyInfo<Guid> UserIdProperty = RegisterProperty<Guid>(p => p.UserId);
        public Guid UserId
        {
            get => ReadProperty(UserIdProperty);
            set => LoadProperty(UserIdProperty, value);
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

        /// <summary>
        /// Resets the users SecurityStamp. When it expires, user will be required to log-in.<br/>
        /// Default SecurityStamp expiration time = 30 min
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="factory"></param>
        /// <returns></returns>
        public static (bool Success, string ErrorMessage) LogUserOut(Guid userId, IDataPortalFactory factory)
        {
            var cmd = factory.GetPortal<TEUserLogoutCommand>().Create();
            cmd.UserId = userId;
            cmd = factory.GetPortal<TEUserLogoutCommand>().Execute(cmd);
            return (cmd.Success, cmd.ErrorMessage);
        }

        /// <summary>
        /// Resets the users SecurityStamp. When it expires, user will be required to log-in.<br/>
        /// Default SecurityStamp expiration time = 30 min
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="factory"></param>
        /// <returns></returns>
        public static async Task<(bool Success, string ErrorMessage)> LogUserOutAsync(Guid userId, IDataPortalFactory factory)
        {
            var cmd = await factory.GetPortal<TEUserLogoutCommand>().CreateAsync();
            cmd.UserId = userId;
            cmd = await factory.GetPortal<TEUserLogoutCommand>().ExecuteAsync(cmd);
            return (cmd.Success, cmd.ErrorMessage);
        }

        #endregion

        #region Server-side methods

        [Execute]
        protected async Task Execute([Inject] IUserService userService, [Inject] ILogger logger)
        {
            try
            {
                TEIdentityUser user = await userService.GetUserByIdAsync(UserId) ?? throw new Exception("User not found");
                await userService.LogUserOut(UserId);
                Success = true;
            }
            catch (Exception e)
            {
                logger?.LogError(e, "Error logging out User " + UserId.ToString());
                Success = false;
                ErrorMessage = e.Message;
            }
        }

        #endregion
    }
}
