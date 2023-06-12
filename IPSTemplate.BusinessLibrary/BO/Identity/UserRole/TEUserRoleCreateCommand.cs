using Core.DAL.Infrastructure;
using Core.Library.Base;
using Csla;
using IPSTemplate.BusinessLibrary.Interfaces;

namespace IPSTemplate.BusinessLibrary.BO.Identity.UserRole
{
    [Serializable]
    public class TEUserRoleCreateCommand : CslaCommandBase<TEUserRoleCreateCommand, Entity>
    {
        #region Properties

        public static readonly PropertyInfo<string> RoleNameProperty = RegisterProperty<string>(p => p.RoleName);
        public string RoleName
        {
            get => ReadProperty(RoleNameProperty);
            set => LoadProperty(RoleNameProperty, value);
        }

        #endregion

        #region Client-side methods

        public static async Task CreateNewRoleAsync(string roleName, IDataPortalFactory factory)
        {
            TEUserRoleCreateCommand cmd = await factory.GetPortal<TEUserRoleCreateCommand>().CreateAsync();
            cmd.RoleName = roleName;
            cmd = await factory.GetPortal<TEUserRoleCreateCommand>().ExecuteAsync(cmd);
        }
        public static void CreateNewRole(string roleName, IDataPortalFactory factory)
        {
            TEUserRoleCreateCommand cmd = factory.GetPortal<TEUserRoleCreateCommand>().Create();
            cmd.RoleName = roleName;
            cmd = factory.GetPortal<TEUserRoleCreateCommand>().Execute(cmd);
        }

        #endregion

        #region Server-side methods

        [Execute]
        protected async Task ExecuteAsync([Inject] IUserService userService)
        {
            if (!string.IsNullOrEmpty(RoleName))
                await userService.CreateNewRole(RoleName);
        }

        #endregion
    }
}
