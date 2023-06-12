using Core.DAL.Infrastructure;
using Core.Library.Base;
using Core.Library.Extensions;
using Csla;
using Csla.Core;
using IPSTemplate.BusinessLibrary.StandardCollections;

namespace IPSTemplate.BusinessLibrary.BO.Identity.User
{
    [Serializable]
    public class TEUserInfo : CslaReadOnlyBase<TEUserInfo, Entity>
    {
        #region Properties

        public static readonly PropertyInfo<string> UsernameProperty = RegisterProperty<string>(p => p.Username);
        //[Display(ResourceType = typeof(Properties.Resources), Name = nameof(User_Username))]
        public string Username
        {
            get => GetProperty(UsernameProperty);
            set => LoadProperty(UsernameProperty, value);
        }

        public static readonly PropertyInfo<string> EmailProperty = RegisterProperty<string>(p => p.Email);
        //[Display(ResourceType = typeof(Properties.Resources), Name = nameof(User_Email))]
        public string Email
        {
            get => GetProperty(EmailProperty);
            set => LoadProperty(EmailProperty, value);
        }

        public static readonly PropertyInfo<string> PhoneNumberProperty = RegisterProperty<string>(p => p.PhoneNumber);
        //[Display(ResourceType = typeof(Properties.Resources), Name = nameof(User_PhoneNumber))]
        public string PhoneNumber
        {
            get => GetProperty(PhoneNumberProperty);
            set => LoadProperty(PhoneNumberProperty, value);
        }

        public static readonly PropertyInfo<bool> EmailConfirmedProperty = RegisterProperty<bool>(p => p.EmailConfirmed);
        //[Display(ResourceType = typeof(Properties.Resources), Name = nameof(User_EmailConfirmed))]
        public bool EmailConfirmed
        {
            get => GetProperty(EmailConfirmedProperty);
            set => LoadProperty(EmailConfirmedProperty, value);
        }

        public static readonly PropertyInfo<bool> PhoneNumberConfirmedProperty = RegisterProperty<bool>(p => p.PhoneNumberConfirmed);
        //[Display(ResourceType = typeof(Properties.Resources), Name = nameof(User_PhoneNumberConfirmed))]
        public bool PhoneNumberConfirmed
        {
            get => GetProperty(PhoneNumberConfirmedProperty);
            set => LoadProperty(PhoneNumberConfirmedProperty, value);
        }

        public static readonly PropertyInfo<string> NameProperty = RegisterProperty<string>(p => p.Name);
        //[Display(ResourceType = typeof(Properties.Resources), Name = nameof(User_Name))]
        public string Name
        {
            get => GetProperty(NameProperty);
            set => LoadProperty(NameProperty, value);
        }

        public static readonly PropertyInfo<string> SurnameProperty = RegisterProperty<string>(p => p.Surname);
        //[Display(ResourceType = typeof(Properties.Resources), Name = nameof(User_Surname))]
        public string Surname
        {
            get => GetProperty(SurnameProperty);
            set => LoadProperty(SurnameProperty, value);
        }

        public static readonly PropertyInfo<bool> ActiveProperty = RegisterProperty<bool>(p => p.Active);
        //[Display(ResourceType = typeof(Properties.Resources), Name = nameof(User_Active))]
        public bool Active
        {
            get => GetProperty(ActiveProperty);
            set => LoadProperty(ActiveProperty, value);
        }

        public static readonly PropertyInfo<MobileList<string>> UserRolesProperty = RegisterProperty<MobileList<string>>(p => p.UserRoles);
        //[Display(ResourceType = typeof(Properties.Resources), Name = nameof(User_UserRoles))]
        public MobileList<string> UserRoles
        {
            get => GetProperty(UserRolesProperty);
            set => LoadProperty(UserRolesProperty, value);
        }

        public static readonly PropertyInfo<DateTime> BirthDateProperty = RegisterProperty<DateTime>(p => p.BirthDate);
        //[Display(ResourceType = typeof(Properties.Resources), Name = nameof(User_BirthDate))]
        public DateTime BirthDate
        {
            get => GetProperty(BirthDateProperty);
            set => LoadProperty(BirthDateProperty, value);
        }

        public static readonly PropertyInfo<string> NationalityProperty = RegisterProperty<string>(p => p.Nationality);
        //[Display(ResourceType = typeof(Properties.Resources), Name = nameof(User_BirthDate))]
        public string Nationality
        {
            get => GetProperty(NationalityProperty);
            set => LoadProperty(NationalityProperty, value);
        }

        //[Display(ResourceType = typeof(Properties.Resources), Name = nameof(User_UserRoles))]
        public string UserRolesString
        {
            get
            {
                return (UserRoles == null ? "" : string.Join(", ", UserRoles.Select(r => TEUserRole.GetUserFriendlyName(r)).ToList()));
            }
        }

        public string DisplayName
        {
            get => $"{Name} {Surname}".Trim();
        }

        public string DisplayNameLong
        {
            get => $"{Name} {Surname} ({Username})".Trim();
        }

        #endregion
    }
}
