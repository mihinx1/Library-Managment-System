using Core.DAL.Infrastructure;
using Core.Library.Base;
using Csla;
using Csla.Core;
using IPSTemplate.BusinessLibrary.Interfaces;

namespace IPSTemplate.BusinessLibrary.BO.Identity.User
{
    [Serializable]
    public class TEUserGetCommand : CslaCommandBase<TEUserGetCommand, Entity>
    {
        #region Properties

        public static readonly PropertyInfo<Guid> UserIdProperty = RegisterProperty<Guid>(p => p.UserId);
        public Guid UserId
        {
            get => ReadProperty(UserIdProperty);
            set => LoadProperty(UserIdProperty, value);
        }

        public static readonly PropertyInfo<TEUserBO> UserProperty = RegisterProperty<TEUserBO>(p => p.User);
        public TEUserBO User
        {
            get => ReadProperty(UserProperty);
            set => LoadProperty(UserProperty, value);
        }

        #endregion

        #region Client-side methods

        public static async Task<TEUserBO> GetUserAsync(Guid userId, IDataPortalFactory factory)
        {
            TEUserGetCommand cmd = await factory.GetPortal<TEUserGetCommand>().CreateAsync();
            cmd.UserId = userId;
            cmd = await factory.GetPortal<TEUserGetCommand>().ExecuteAsync(cmd);
            return cmd.User ?? throw new DataNotFoundException($"User with Id {userId} was not found!");
        }
        public static TEUserBO GetUser(Guid userId, IDataPortalFactory factory)
        {
            TEUserGetCommand cmd = factory.GetPortal<TEUserGetCommand>().Create();
            cmd.UserId = userId;
            cmd = factory.GetPortal<TEUserGetCommand>().Execute(cmd);
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
                var boUser = await TEUserBO.NewItemAsync(factory);
                boUser.Id = user.Id; // Guid.Parse(user.Id);
                boUser.Username = user.UserName;
                boUser.Email = user.Email;
                boUser.EmailConfirmed = user.EmailConfirmed;
                boUser.Name = user.Name;
                boUser.Surname = user.Surname;
                boUser.PhoneNumber = user.PhoneNumber;
                boUser.PhoneNumberConfirmed = user.PhoneNumberConfirmed;
                boUser.UserRoles = new MobileList<string>(roles);
                boUser.Active = user.Active;
                boUser.Nationality = user.Nationality;
                boUser.BirthDate = user.BirthDate;
                boUser.MarkOld();
                boUser.Mark_Clean();
                this.User = boUser;
            }
        }

        #endregion
    }
}
