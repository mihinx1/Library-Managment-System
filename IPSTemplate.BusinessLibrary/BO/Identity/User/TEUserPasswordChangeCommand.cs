using Core.DAL.Infrastructure;
using Core.Library.Base;
using Csla;
using IPSTemplate.BusinessLibrary.Interfaces;

namespace IPSTemplate.BusinessLibrary.BO.Identity.User
{
    [Serializable]
    public class TEUserPasswordChangeCommand : CslaCommandBase<TEUserPasswordChangeCommand, Entity>
    {
        #region Properties

        public static readonly PropertyInfo<Guid> UserIdProperty = RegisterProperty<Guid>(p => p.UserId);
        public Guid UserId
        {
            get => ReadProperty(UserIdProperty);
            set => LoadProperty(UserIdProperty, value);
        }

        public static readonly PropertyInfo<string> PasswordProperty = RegisterProperty<string>(p => p.Password);
        public string Password
        {
            get => ReadProperty(PasswordProperty);
            set => LoadProperty(PasswordProperty, value);
        }

        #endregion

        #region Client-side methods

        public static async Task SetPasswordAsync(Guid userId, string password, IDataPortalFactory factory)
        {
            TEUserPasswordChangeCommand cmd = await factory.GetPortal<TEUserPasswordChangeCommand>().CreateAsync();
            cmd.UserId = userId;
            cmd.Password = password;
            cmd = await factory.GetPortal<TEUserPasswordChangeCommand>().ExecuteAsync(cmd);
        }

        public static void SetPassword(Guid userId, string password, IDataPortalFactory factory)
        {
            TEUserPasswordChangeCommand cmd = factory.GetPortal<TEUserPasswordChangeCommand>().Create();
            cmd.UserId = userId;
            cmd.Password = password;
            cmd = factory.GetPortal<TEUserPasswordChangeCommand>().Execute(cmd);
        }

        #endregion

        #region Server-side methods

        [Execute]
        protected async Task ExecuteAsync([Inject] IUserService userService)
        {
            await userService.SetUserPassword(UserId, Password);
        }

        #endregion
    }
}
