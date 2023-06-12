using Core.DAL.Infrastructure;
using Core.Library.Base;
using Csla;
using IPSTemplate.BusinessLibrary.Interfaces;

namespace IPSTemplate.BusinessLibrary.BO.Identity.User
{
    [Serializable]
    public class TEUserGetListCommand : CslaCommandBase<TEUserGetListCommand, Entity>
    {
        #region Properties

        public static readonly PropertyInfo<MobileCslaRequest> RequestProperty = RegisterProperty<MobileCslaRequest>(p => p.Request);
        public MobileCslaRequest Request
        {
            get => ReadProperty(RequestProperty);
            set => LoadProperty(RequestProperty, value);
        }

        public static readonly PropertyInfo<TEUserRL> UserListProperty = RegisterProperty<TEUserRL>(p => p.UserList);
        public TEUserRL UserList
        {
            get => ReadProperty(UserListProperty);
            set => LoadProperty(UserListProperty, value);
        }

        public static readonly PropertyInfo<int> TotalCountProperty = RegisterProperty<int>(p => p.TotalCount);
        public int TotalCount
        {
            get => ReadProperty(TotalCountProperty);
            set => LoadProperty(TotalCountProperty, value);
        }

        public static readonly PropertyInfo<bool> LoadRolesProperty = RegisterProperty<bool>(p => p.LoadRoles);
        public bool LoadRoles
        {
            get => ReadProperty(LoadRolesProperty);
            set => LoadProperty(LoadRolesProperty, value);
        }

        #endregion

        #region Client-side methods

        public static async Task<(TEUserRL Data, int TotalCount)> GetUserListAsync(MobileCslaRequest request, IDataPortalFactory factory, bool loadRoles = true)
        {
            TEUserGetListCommand cmd = await factory.GetPortal<TEUserGetListCommand>().CreateAsync();
            cmd.Request = request;
            cmd.LoadRoles = loadRoles;
            cmd = await factory.GetPortal<TEUserGetListCommand>().ExecuteAsync(cmd);
            return (cmd.UserList, cmd.TotalCount);
        }
        public static (TEUserRL Data, int TotalCount) GetUserList(MobileCslaRequest request, IDataPortalFactory factory, bool loadRoles = true)
        {
            TEUserGetListCommand cmd = factory.GetPortal<TEUserGetListCommand>().Create();
            cmd.Request = request;
            cmd.LoadRoles = loadRoles;
            cmd = factory.GetPortal<TEUserGetListCommand>().Execute(cmd);
            return (cmd.UserList, cmd.TotalCount);
        }

        #endregion

        #region Server-side methods

        [Execute]
        protected async Task ExecuteAsync([Inject] IUserService userService, [Inject] IDataPortalFactory factory)
        {
            string? usernameFilter = Request?.Filters?.FirstOrDefault(f => f.Member == nameof(TEUserInfo.Username))?.Value?.ToString();
            string? emailFilter = Request?.Filters?.FirstOrDefault(f => f.Member == nameof(TEUserInfo.Email))?.Value?.ToString();
            string? phoneFilter = Request?.Filters?.FirstOrDefault(f => f.Member == nameof(TEUserInfo.PhoneNumber))?.Value?.ToString();
            string? nameFilter = Request?.Filters?.FirstOrDefault(f => f.Member == nameof(TEUserInfo.Name))?.Value?.ToString();
            string? surnameFilter = Request?.Filters?.FirstOrDefault(f => f.Member == nameof(TEUserInfo.Surname))?.Value?.ToString();
            string? rolesFilter = Request?.Filters?.FirstOrDefault(f => f.Member == nameof(TEUserInfo.UserRolesString))?.Value?.ToString();
            string? nationalityFilter = Request?.Filters?.FirstOrDefault(f => f.Member == nameof(TEUserInfo.UserRolesString))?.Value?.ToString();
            bool? activeFilter = (bool?)Request?.Filters?.FirstOrDefault(f => f.Member == nameof(TEUserInfo.Active))?.Value;
            DateTime? birthdateFilter = (DateTime?)Request?.Filters?.FirstOrDefault(f => f.Member == nameof(TEUserInfo.BirthDate))?.Value;
            var page = Request?.Page ?? 1;
            var pageSize = Request?.PageSize ?? int.MaxValue;

            var users = await userService.GetUsersAsync(new UserFilter(activeFilter, page, pageSize, usernameFilter, emailFilter, phoneFilter, nameFilter, surnameFilter, rolesFilter, nationalityFilter, birthdateFilter));
            TotalCount = users.TotalCount;
            UserList = await factory.GetPortal<TEUserRL>().CreateAsync();
            UserList.EnableEditing(true);
            using (UserList.SuppressListChangedEvents)
            {
                foreach (var user in users.Users)
                {
                    var roles = new List<string>();
                    if (LoadRoles)
                        roles = await userService.GetUsersRoles(user);

                    var infoUser = await factory.GetPortal<TEUserInfo>().CreateAsync();
                    infoUser.Id = user.Id;
                    infoUser.Username = user.UserName;
                    infoUser.Email = user.Email;
                    infoUser.EmailConfirmed = user.EmailConfirmed;
                    infoUser.Name = user.Name;
                    infoUser.Surname = user.Surname;
                    infoUser.PhoneNumber = user.PhoneNumber;
                    infoUser.PhoneNumberConfirmed = user.PhoneNumberConfirmed;
                    infoUser.UserRoles = new Csla.Core.MobileList<string>(roles);
                    infoUser.Active = user.Active;
                    infoUser.Nationality = user.Nationality;
                    infoUser.BirthDate = user.BirthDate;
                    UserList.Add(infoUser);
                }
            }
            UserList.EnableEditing(false);
        }

        #endregion
    }
}
