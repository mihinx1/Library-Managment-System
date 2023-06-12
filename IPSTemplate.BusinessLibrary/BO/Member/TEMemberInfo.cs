using Core.Library.Base;
using Csla;
using IPSTemplate.Dal.Models;
using System.ComponentModel.DataAnnotations;

namespace IPSTemplate.BusinessLibrary.BO.Member
{
    [Serializable]
    public class TEMemberInfo : CslaReadOnlyBase<TEMemberInfo, TEMember>
    {
        #region Properties

        public static readonly PropertyInfo<string> FirstNameProperty = RegisterProperty<string>(p => p.FirstName);
        [Display(Name = "Ime")]
        public string FirstName
        {
            get => GetProperty(FirstNameProperty);
            set => LoadProperty(FirstNameProperty, value);
        }

        public static readonly PropertyInfo<string> LastNameProperty = RegisterProperty<string>(p => p.LastName);
        [Display(Name = "Priimek")]
        public string LastName
        {
            get => GetProperty(LastNameProperty);
            set => LoadProperty(LastNameProperty, value);
        }

        public static readonly PropertyInfo<int> BirthYearProperty = RegisterProperty<int>(p => p.BirthYear);
        [Display(Name = "Letnica rojstva")]
        public int BirthYear
        {
            get => GetProperty(BirthYearProperty);
            set => LoadProperty(BirthYearProperty, value);
        }

        public static readonly PropertyInfo<string> NationalityProperty = RegisterProperty<string>(p => p.Nationality);
        [Display(Name = "Nacionalnost")]
        public string Nationality
        {
            get => GetProperty(NationalityProperty);
            set => LoadProperty(NationalityProperty, value);
        }

        public static readonly PropertyInfo<string> GenderProperty = RegisterProperty<string>(p => p.Gender);
        [Display(Name = "Spol")]
        public string Gender
        {
            get => GetProperty(GenderProperty);
            set => LoadProperty(GenderProperty, value);
        }

        public static readonly PropertyInfo<string> PhoneNumberProperty = RegisterProperty<string>(p => p.PhoneNumber);
        [Display(Name = "Telefonska številka")]
        public string PhoneNumber
        {
            get => GetProperty(PhoneNumberProperty);
            set => LoadProperty(PhoneNumberProperty, value);
        }

        public static readonly PropertyInfo<string> EmailProperty = RegisterProperty<string>(p => p.Email);
        [Display(Name = "Email")]
        public string Email
        {
            get => GetProperty(EmailProperty);
            set => LoadProperty(EmailProperty, value);
        }

        public string ShowAs
        {
            get => FirstName + " " + LastName;
        }
        #endregion
    }
}
