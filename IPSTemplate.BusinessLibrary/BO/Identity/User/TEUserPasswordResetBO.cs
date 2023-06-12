using System.ComponentModel.DataAnnotations;
using Core.DAL.Infrastructure;
using Core.Library.Base;
using Csla;

namespace IPSTemplate.BusinessLibrary.BO.Identity.User
{
    [Serializable]
    public class TEUserPasswordResetBO : CslaBusinessBase<TEUserPasswordResetBO, Entity>
    {
        #region Properties

        public static readonly PropertyInfo<Guid> UserIdProperty = RegisterProperty<Guid>(p => p.UserId);
        public Guid UserId
        {
            get => GetProperty(UserIdProperty);
            set => SetProperty(UserIdProperty, value);
        }

        public static readonly PropertyInfo<string> ResetTokenProperty = RegisterProperty<string>(p => p.ResetToken);
        public string ResetToken
        {
            get => GetProperty(ResetTokenProperty);
            set => SetProperty(ResetTokenProperty, value);
        }

        public static readonly PropertyInfo<string> CurrentPasswordProperty = RegisterProperty<string>(p => p.CurrentPassword);
        [Required]
        [DataType(DataType.Password)]
        [LocalizedStringLength(50)]
        //[Display(ResourceType = typeof(Properties.Resources), Name = nameof(User_CurrentPassword))]
        public string CurrentPassword
        {
            get => GetProperty(CurrentPasswordProperty);
            set => SetProperty(CurrentPasswordProperty, value);
        }

        public static readonly PropertyInfo<string> NewPasswordProperty = RegisterProperty<string>(p => p.NewPassword);
        [Required]
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
        [Required]
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

        public async Task<(bool Success, string ErrorMessage)> ResetPasswordAsync(IDataPortalFactory factory)
        {
            return await TEUserPasswordResetCommand.ResetPasswordAsync(this, factory);
        }

        protected override async Task<TEUserPasswordResetBO> SaveAsync(bool forceUpdate, object userState, bool isSync)
        {
            Mark_Clean();
            MarkOld();
            return this;
        }


        #endregion
    }
}
