using Core.DAL.Infrastructure;
using Core.Library.Base;
using Csla;
using System.ComponentModel.DataAnnotations;

namespace IPSTemplate.BusinessLibrary.BO.Identity.User
{
    [Serializable]
    public class TEUserForgottenPasswordBO : CslaBusinessBase<TEUserForgottenPasswordBO, Entity>
    {
        #region Properties

        public static readonly PropertyInfo<Guid> UserIdProperty = RegisterProperty<Guid>(p => p.UserId);
        public Guid UserId
        {
            get => GetProperty(UserIdProperty);
            set => SetProperty(UserIdProperty, value);
        }

        public static readonly PropertyInfo<string> UserEmailProperty = RegisterProperty<string>(p => p.UserEmail);
        [Required]
        [DataType(DataType.EmailAddress)]
        [LocalizedStringLength(50)]
        //[Display(ResourceType = typeof(Properties.Resources), Name = nameof(User_ResetPasswordEmail))]
        [RegularExpression(RegularExpressions.Email.EmailAddress)]
        public string UserEmail
        {
            get => GetProperty(UserEmailProperty);
            set => SetProperty(UserEmailProperty, value);
        }

        #endregion

        #region Client-side methods

        //public async Task<(bool Success, string ErrorMessage)> GenerateTokenAndSendMail(IDataPortalFactory factory)
        //{
        //    return await TEUserSendPasswordResetLinkCommand.SendPasswordResetLinkAsync(this, factory);
        //}

        protected override async Task<TEUserForgottenPasswordBO> SaveAsync(bool forceUpdate, object userState, bool isSync)
        {
            Mark_Clean();
            MarkOld();
            return this;
        }

        #endregion
    }
}
