using Core.DAL.Infrastructure;
using Core.Library.Base;
using Csla;
using IPSTemplate.BusinessLibrary.Interfaces;
using IPSTemplate.Dal.Models.Identity;

namespace IPSTemplate.BusinessLibrary.BO.Identity.User
{
    [Serializable]
    public class TEUserSaveCommand : CslaCommandBase<TEUserSaveCommand, Entity>
    {
        #region Properties

        public static readonly PropertyInfo<TEUserBO> UserProperty = RegisterProperty<TEUserBO>(p => p.User);
        public TEUserBO User
        {
            get => ReadProperty(UserProperty);
            set => LoadProperty(UserProperty, value);
        }

        #endregion

        #region Client-side methods

        public static async Task<TEUserBO> SaveUserAsync(TEUserBO user, IDataPortalFactory factory)
        {
            TEUserSaveCommand cmd = await factory.GetPortal<TEUserSaveCommand>().CreateAsync();
            cmd.User = user;
            cmd = await factory.GetPortal<TEUserSaveCommand>().ExecuteAsync(cmd);
            return cmd.User;
        }
        public static TEUserBO SaveUser(TEUserBO user, IDataPortalFactory factory)
        {
            TEUserSaveCommand cmd = factory.GetPortal<TEUserSaveCommand>().Create();
            cmd.User = user;
            cmd = factory.GetPortal<TEUserSaveCommand>().Execute(cmd);
            return cmd.User;
        }

        #endregion

        #region Server-side methods

        [Execute]
        protected async Task ExecuteAsync([Inject] IUserService userService)
        {
            if (User.IsNew)
            {
                await userService.AddUserAsync(
                    new TEIdentityUser
                    {
                        Id = User.Id,//.ToString(),
                        Email = User.Email,
                        EmailConfirmed = User.EmailConfirmed,
                        Name = User.Name,
                        Surname = User.Surname,
                        PhoneNumber = User.PhoneNumber,
                        PhoneNumberConfirmed = User.PhoneNumberConfirmed,
                        UserName = User.Username,
                        Active = User.Active,
                        Nationality = User.Nationality,
                        BirthDate = User.BirthDate
                    });
            }
            else
            {
                await userService.UpdateUserAsync(
                    new TEIdentityUser
                    {
                        Id = User.Id,//.ToString(),
                        Email = User.Email,
                        EmailConfirmed = User.EmailConfirmed,
                        Name = User.Name,
                        Surname = User.Surname,
                        PhoneNumber = User.PhoneNumber,
                        PhoneNumberConfirmed = User.PhoneNumberConfirmed,
                        UserName = User.Username,
                        Active = User.Active,
                        Nationality = User.Nationality,
                        BirthDate = User.BirthDate
                    });
            }
        }

        #endregion
    }
}
