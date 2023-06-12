using System.ComponentModel.DataAnnotations;
using Core.DAL.Infrastructure;
using Core.Library.Base;
using Csla;

namespace IPSTemplate.BusinessLibrary.BO.Identity.User
{
    [Serializable]
    public class TEUserPasswordChangeBO : CslaBusinessBase<TEUserPasswordChangeBO, Entity>
    {
        #region Properties

        public static readonly PropertyInfo<Guid> UserIdProperty = RegisterProperty<Guid>(p => p.UserId);
        [Required]
        public Guid UserId
        {
            get => GetProperty(UserIdProperty);
            set => SetProperty(UserIdProperty, value);
        }

        public static readonly PropertyInfo<string> NewPasswordProperty = RegisterProperty<string>(p => p.NewPassword);
        [DataType(DataType.Password)]
        [LocalizedStringLength(50)]
        //[Display(ResourceType = typeof(Properties.Resources), Name = nameof(User_NewPassword))]
        [RegularExpression(RegularExpressions.Password.LoginPassword)]
        public string NewPassword
        {
            get => GetProperty(NewPasswordProperty);
            set => SetProperty(NewPasswordProperty, value);
        }

        public static readonly PropertyInfo<string> ConfirmPasswordProperty = RegisterProperty<string>(p => p.ConfirmPassword);
        [DataType(DataType.Password)]
        [LocalizedStringLength(50)]
        //[Display(ResourceType = typeof(Properties.Resources), Name = nameof(User_ConfirmPassword))]
        [Compare(nameof(NewPassword))]
        public string ConfirmPassword
        {
            get => GetProperty(ConfirmPasswordProperty);
            set => SetProperty(ConfirmPasswordProperty, value);
        }

        #endregion

        #region Client-side methods
        public static new async Task<TEUserPasswordChangeBO> GetItemAsync(Guid Id, IDataPortalFactory factory)
        {
            var user = await TEUserPasswordChangeBO.NewItemAsync(factory);
            user.Id = Id;
            return user;
        }

        protected override async Task<TEUserPasswordChangeBO> SaveAsync(bool forceUpdate, object userState, bool isSync)
        {
            if (isSync)
            {
                TEUserPasswordChangeCommand.SetPassword(Id, NewPassword, ApplicationContext.GetRequiredService<IDataPortalFactory>());
            }
            else
            {
                await TEUserPasswordChangeCommand.SetPasswordAsync(Id, NewPassword, ApplicationContext.GetRequiredService<IDataPortalFactory>());
            }
            Mark_Clean();
            MarkOld();
            return this;
        }

        #endregion
    }
}
