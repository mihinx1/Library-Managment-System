using Core.DAL.Infrastructure;
using Core.Library.Base;
using Csla;
using Csla.Core;
using IPSTemplate.BusinessLibrary.Interfaces;

namespace IPSTemplate.BusinessLibrary.BO.Identity.User
{
    [Serializable]
    public class TEUserGetInfoCommand : CslaCommandBase<TEUserGetInfoCommand, Entity>
    {
        #region Properties

        public static readonly PropertyInfo<Guid> UserIdProperty = RegisterProperty<Guid>(p => p.UserId);
        public Guid UserId
        {
            get => ReadProperty(UserIdProperty);
            set => LoadProperty(UserIdProperty, value);
        }

        public static readonly PropertyInfo<TEUserInfo> UserProperty = RegisterProperty<TEUserInfo>(p => p.User);
        public TEUserInfo User
        {
            get => ReadProperty(UserProperty);
            set => LoadProperty(UserProperty, value);
        }

        #endregion

        #region Client-side methods

        public static async Task<TEUserInfo> GetUserAsync(Guid userId, IDataPortalFactory factory)
        {
            TEUserGetInfoCommand cmd = await factory.GetPortal<TEUserGetInfoCommand>().CreateAsync();
            cmd.UserId = userId;
            cmd = await factory.GetPortal<TEUserGetInfoCommand>().ExecuteAsync(cmd);
            return cmd.User ?? throw new DataNotFoundException($"User with Id {userId} was not found!");
        }

        public static TEUserInfo GetUser(Guid userId, IDataPortalFactory factory)
        {
            TEUserGetInfoCommand cmd = factory.GetPortal<TEUserGetInfoCommand>().Create();
            cmd.UserId = userId;
            cmd = factory.GetPortal<TEUserGetInfoCommand>().Execute(cmd);
            return cmd.User ?? throw new DataNotFoundException($"User with Id {userId} was not found!");
        }

        #endregion

        #region Server-side methods

        [Execute]
        protected async Task ExecuteAsync([Inject] IUserService userService, [Inject] IDataPortalFactory factory)
        {
            var user = await userService.GetUserByIdAsync(UserId);
            var roles = await userService.GetUsersRoles(user);

            if (user != null)
            {
                var info = await factory.GetPortal<TEUserInfo>().CreateAsync();
                info.Id = user.Id;
                info.Username = user.UserName;
                info.Email = user.Email;
                info.EmailConfirmed = user.EmailConfirmed;
                info.Name = user.Name;
                info.Surname = user.Surname;
                info.PhoneNumber = user.PhoneNumber;
                info.PhoneNumberConfirmed = user.PhoneNumberConfirmed;
                info.UserRoles = new MobileList<string>(roles);
                info.Active = user.Active;
                info.Nationality = user.Nationality;
                info.BirthDate = user.BirthDate;
                this.User = info;
            }
        }

        #endregion
    }
}
