using System.Data;
using Core.DAL.Infrastructure;
using Core.DALEF;
using Core.Library.Base;
using Csla;
using Csla.Core;
using IPSTemplate.BusinessLibrary.Interfaces;

namespace IPSTemplate.BusinessLibrary.BO.Identity.UserRole
{
    [Serializable]
    public class TEUserRoleGetListCommand : CslaCommandBase<TEUserRoleGetListCommand, Entity>
    {
        #region Properties

        public static readonly PropertyInfo<Guid?> UserIdProperty = RegisterProperty<Guid?>(p => p.UserId);
        public Guid? UserId
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

        public static async Task<List<string>> GetUserRolesListAsync(Guid userId, IDataPortalFactory factory)
        {
            TEUserRoleGetListCommand cmd = await factory.GetPortal<TEUserRoleGetListCommand>().CreateAsync();
            cmd.UserId = userId;
            cmd = await factory.GetPortal<TEUserRoleGetListCommand>().ExecuteAsync(cmd);
            return cmd.UserRolesList;
        }
        public static List<string> GetUserRolesList(Guid userId, IDataPortalFactory factory)
        {
            TEUserRoleGetListCommand cmd = factory.GetPortal<TEUserRoleGetListCommand>().Create();
            cmd.UserId = userId;
            cmd = factory.GetPortal<TEUserRoleGetListCommand>().Execute(cmd);
            return cmd.UserRolesList;
        }

        public static async Task<List<string>> GetAllUserRolesAsync(IDataPortalFactory factory)
        {
            TEUserRoleGetListCommand cmd = await factory.GetPortal<TEUserRoleGetListCommand>().CreateAsync();
            cmd = await factory.GetPortal<TEUserRoleGetListCommand>().ExecuteAsync(cmd);
            return cmd.UserRolesList;
        }
        public static List<string> GetAllUserRoles(IDataPortalFactory factory)
        {
            TEUserRoleGetListCommand cmd = factory.GetPortal<TEUserRoleGetListCommand>().Create();
            cmd = factory.GetPortal<TEUserRoleGetListCommand>().Execute(cmd);
            return cmd.UserRolesList;
        }

        #endregion

        #region Server-side methods

        [Execute]
        protected async Task ExecuteAsync([Inject] IUserService userService)
        {
            if (UserId.HasValue)
            {
                var roles = await userService.GetUsersRoles(UserId.Value);
                UserRolesList = new MobileList<string>(roles.OrderBy(r => r));
            }
            else
            {
                var roles = userService.GetAllRoles();
                UserRolesList = new MobileList<string>(roles.OrderBy(r => r));
            }
        }

        #endregion
    }
}
