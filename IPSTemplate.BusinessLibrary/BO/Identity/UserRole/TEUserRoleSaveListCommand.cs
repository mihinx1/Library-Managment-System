using Core.DAL.Infrastructure;
using Core.Library.Base;
using Csla;
using Csla.Core;
using IPSTemplate.BusinessLibrary.Interfaces;

namespace IPSTemplate.BusinessLibrary.BO.Identity.UserRole
{
    [Serializable]
    public class TEUserRoleSaveListCommand : CslaCommandBase<TEUserRoleSaveListCommand, Entity>
    {
        #region Properties

        public static readonly PropertyInfo<Guid> UserIdProperty = RegisterProperty<Guid>(p => p.UserId);
        public Guid UserId
        {
            get => ReadProperty(UserIdProperty);
            set => LoadProperty(UserIdProperty, value);
        }

        public static readonly PropertyInfo<MobileList<string>> UserRolesListProperty = RegisterProperty<MobileList<string>>(p => p.UserRolesList);
        public MobileList<string> UserRolesList
        {
            get => ReadProperty(UserRolesListProperty);
            set => LoadProperty(UserRolesListProperty, value);
        }

        #endregion

        #region Client-side methods

        public static async Task SaveUserRolesListAsync(Guid userId, IEnumerable<string> roles, IDataPortalFactory factory)
        {
            TEUserRoleSaveListCommand cmd = await factory.GetPortal<TEUserRoleSaveListCommand>().CreateAsync();
            cmd.UserId = userId;
            cmd.UserRolesList = new MobileList<string>(roles);
            cmd = await factory.GetPortal<TEUserRoleSaveListCommand>().ExecuteAsync(cmd);
        }
        public static void SaveUserRolesList(Guid userId, IEnumerable<string> roles, IDataPortalFactory factory)
        {
            TEUserRoleSaveListCommand cmd = factory.GetPortal<TEUserRoleSaveListCommand>().Create();
            cmd.UserId = userId;
            cmd.UserRolesList = new MobileList<string>(roles);
            cmd = factory.GetPortal<TEUserRoleSaveListCommand>().Execute(cmd);
        }

        #endregion

        #region Server-side methods

        [Execute]
        protected async Task ExecuteAsync([Inject] IUserService userService)
        {
            await userService.SaveUsersRoles(UserId, UserRolesList);
        }

        #endregion
    }
}
